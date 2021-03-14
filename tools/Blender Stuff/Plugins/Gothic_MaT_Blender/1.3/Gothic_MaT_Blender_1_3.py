bl_info = {
    "name": "Gothic Materials and Textures Blender",
    "description": "Makes life easier for Gothic material export",
    "author": "Diego",
    "version": (1, 3, 0),
    "blender": (2, 78, 0),
    "location": "3D View > Tools",
    "warning": "", # used for warning icon and text in addons panel
    "wiki_url": "",
    "tracker_url": "",
    "category": "Development"
}


import bpy
# if not blenders bundled python is used, packages might not be installed
try:
    from mathutils import Color 
except ImportError:
    raise ImportError('Package mathutils needed, but not installed') 
    
try:
    import numpy
except ImportError:
    raise ImportError('Package numpy needed, but not installed') 
    
try:
    import os.path
except ImportError:
    raise ImportError('Package os needed, but not installed') 
    
try:
    import colorsys 
except ImportError:
    raise ImportError('Package colorsys needed, but not installed') 
       


from bpy.props import (StringProperty,
                       BoolProperty,
                       IntProperty,
                       FloatProperty,
                       EnumProperty,
                       PointerProperty,
                       )
from bpy.types import (Panel,
                       Operator,
                       PropertyGroup,
                       )


# ------------------------------------------------------------------------
#    store properties in the active scene
# ------------------------------------------------------------------------


class GothicMaterialSettings(PropertyGroup):

    apply_to_selected_only = BoolProperty(
        name="Only Selected Objects",
        description="Affect only selected objects rather than all (unhidden) objects in the scene",
        default = True
        )
        
    keep_existing_materials = BoolProperty(
        name="Keep Existing Slots",
        description="Keep existing material slots if their texture does not occur and only add new on top",
        default = True
        )
        
    set_transparency = BoolProperty(
        name="Transparency",
        description="Alpha channel will affect transparency in textured view",
        default = True
        )
        
    keep_portals = BoolProperty(
        name="Keep Portals",
        description="Do not overwrite Portal or Ghostoccluder materials",
        default = True
        )
        
    matching_name = BoolProperty(
        name="Use Matching Names",
        description="If exists, use Gothic material with same name as UV-image, even if multiple Gothic materials use this image",
        default = True
        )
        
    isolate_all_layers = BoolProperty(
        name="Isolate in all Layers",
        description="Isolate objects in all layers",
        default = True
        ) 

    pixel_samples = IntProperty(
        name = "Pixels",
        description="Number of pixels taken for material color, becomes very slow for high numbers",
        default = 50,
        min = 1,
        max = 1000
        )
         
    saturation = FloatProperty(
        name = "Saturation",
        description="Makes material colors more or less saturated, 0.5 for unchanged",
        default = 1.,
        min = 0.,
        max = 2.
        )
    
    value = FloatProperty(
        name = "Brigthness",
        description="Changes material color brigthness",
        default = 1.,
        min = 0.,
        max = 2.
        )
               
    searched_material = StringProperty(
        name="Material to Search",
        description="",
        default="unknown",
        maxlen=1024,
        )    

    ambiguous_materials = EnumProperty(
        name="What Material Name for ambiguous Textures?",
        description="What material name for ambiguous textures?",
        items=[ ('first', "First Appearance", ""),
                ('last', "Last Appearance", ""),
                ('generic', "Generic: ambiguous1, ...", ""),
               ]
        )
        
    case = EnumProperty(
        name="Case for Images and Textures",
        description="Case-sensitivity for images and textures",
        items=[ ('keep', "Keep File Case", ""),
                ('upper', "UPPER", ""),
                ('lower', "lower", ""),
               ]
        )
        
    matlib_filepath = StringProperty(
        name="",
        description="Filepath to MatLib.ini",
        default="Filepath to MatLib.ini",
        maxlen=1024,
        subtype='FILE_PATH')

# ------------------------------------------------------------------------
#    operators
# ------------------------------------------------------------------------

# hides all objects that do not have the material specified in the "searched_material" property
# optional: isolate in all layers
class GothicIsolateObjetcs(bpy.types.Operator):
    """Isolate all objects that use this material. Alt+H to reveal"""      # blender will use this as a tooltip for menu items and buttons.
    bl_idname = "object.gothic_isolate_objects"        # unique identifier for buttons and menu items to reference.
    bl_label = "Gothic: Isolate Objects"         # display name in the interface.
    bl_options = {'REGISTER'}  # enable undo for the operator.

    def execute(self, context):        # execute() is called by blender when running the operator.

        scene = context.scene
        searchfor = scene.gothic_tools.searched_material
        isolate_all_layers = scene.gothic_tools.isolate_all_layers
        
        if searchfor == '':
            self.report({'WARNING'}, 'No Material Specified')
            return {'CANCELLED'} 
        
        matindex = bpy.data.materials.find(searchfor)
        if matindex == -1:
            self.report({'WARNING'}, 'Material not found')
            return {'CANCELLED'} 
        else:
            mat = bpy.data.materials[matindex]

        objects_found = []
        # two steps
        # first: check if any objects are found
        for object in bpy.data.objects:
            # if this layer is not supposed to be affected skip
            if not isolate_all_layers:
                if not object.layers[scene.active_layer]:
                    continue
            # if found, add to the list of found objects
            for slot in object.material_slots:
                try:
                    if slot.material == mat:
                        objects_found.append(object)
                        break
                except AttributeError:
                    pass

        # second: if so, hide + deselect all others and reveal + select themselves (in case they were hidden before)
        if objects_found:
            for object in bpy.data.objects:
                if object in objects_found:
                    object.hide = False
                    object.select = True                            
                else:
                    object.hide = True
                    object.select = False
            self.report({'INFO'}, str(len(objects_found)) + ' objects found')  
        else:
            self.report({'INFO'}, 'No objects found')  
            
        return {'FINISHED'}            # this lets blender know the operator finished successfully.


# changes the names of all used images to their filename
# if multiple images use the same file, only one is kept 
# the others will be replaced by this one
class GothicCleanImages(bpy.types.Operator):
    """Rename and replace images not named as their filename"""      # blender will use this as a tooltip for menu items and buttons.
    bl_idname = "context.gothic_clean_images"        # unique identifier for buttons and menu items to reference.
    bl_label = "Gothic: Clean Images and Textures"         # display name in the interface.
    bl_options = {'REGISTER'}  # enable undo for the operator.

    def execute(self, context):        # execute() is called by blender when running the operator.
        
        scene = context.scene
        case = scene.gothic_tools.case
        
        replaced_counter = 0
        renamed_counter = 0
        #rename all images to their filename
        for image in bpy.data.images:
            if image.users:
                filename =  os.path.basename(image.filepath)
                correct_index = bpy.data.images.find(filename)
                
                if correct_index == -1:
                    image.name = filename
                    renamed_counter += 1
                else:
                    correct_image = bpy.data.images[correct_index]
                    if image != correct_image:
                        print(image.name + ' remapped to ' + correct_image.name)
                        image.user_remap(correct_image)
                        replaced_counter +=1
        
        # optional change to lower or upper case
        for image in bpy.data.images:
            if image.users:
                if case.lower() == 'upper':
                    image.name = image.name.upper()
                elif case.lower() == 'lower':
                    image.name = image.name.lower()
        
        self.report({'INFO'}, str(replaced_counter) + ' unlinked, ' + str(renamed_counter) + ' renamed (except for case)')   

        return {'FINISHED'}            # this lets blender know the operator finished successfully.


# Removes suffixes like ".001" and renames textures to image filename
# replaces materials with same name except suffixes
# keeps only one texture per image file, replaces others by this one 
class GothicCleanMaterials(bpy.types.Operator):
    """Remove suffixes as .001 from materials. Note: If object has \"mat\" and \"mat.001\", the slots Will not be merged"""      # blender will use this as a tooltip for menu items and buttons.
    bl_idname = "context.gothic_clean_materials"        # unique identifier for buttons and menu items to reference.
    bl_label = "Gothic: Clean Materials"         # display name in the interface.
    bl_options = {'REGISTER'}  # enable undo for the operator.

    def execute(self, context):        # execute() is called by blender when running the operator.
        
        replaced_counter = 0
        renamed_counter = 0
        # remove suffixes and replace materials that would be named the same
        for mat in bpy.data.materials:
            if mat.users and len(mat.name)>4:
                if mat.name[-4]=='.':
                    try:
                        int(mat.name[-3:])
                        targetname = mat.name[0:-4]
                        index_of_existing = bpy.data.materials.find(targetname)
                        if index_of_existing == -1:
                            mat.name = targetname
                            renamed_counter +=1
                        else:
                            mat.user_remap(bpy.data.materials[index_of_existing])
                            replaced_counter += 1
                    except ValueError:
                            continue
        # change texture name to image file name
        for tex in bpy.data.textures:
            if tex.users:
                try:
                    # may exist already, don't overwrite name yet
                    texname = os.path.basename(tex.image.filepath)
                except AttributeError:
                    print(tex.name + ' has no image')
                    continue
                   
                found_tex_index = bpy.data.textures.find(texname)
                if found_tex_index == -1:
                    tex.name = texname
                else:
                    tex.user_remap(bpy.data.textures[found_tex_index])
        
        self.report({'INFO'}, str(replaced_counter) + ' unlinked, ' + str(renamed_counter) + ' renamed')                
        return {'FINISHED'} 


# takes a sample of pixels and calculates average color for every material with image
class GothicCalcColors(bpy.types.Operator):
    """Calculate all material colors by texture"""      # blender will use this as a tooltip for menu items and buttons.
    bl_idname = "context.gothic_calc_colors"        # unique identifier for buttons and menu items to reference.
    bl_label = "Gothic: Calculate Material Colors"         # display name in the interface.
    bl_options = {'REGISTER'}  # enable undo for the operator.
    

    def execute(self, context):  
        
        scene = context.scene
        set_transparency = scene.gothic_tools.set_transparency
        pixel_samples = scene.gothic_tools.pixel_samples
        value = context.scene.gothic_tools.value
        saturation = context.scene.gothic_tools.saturation
        
        colors_calculated = 0
        too_bright = False
        for material in bpy.data.materials:
            print('Calc color for ' + material.name)
            try:
                if len(material.texture_slots[0].texture.image.pixels):
                    image = material.texture_slots[0].texture.image
                else:
                    continue                 
            except AttributeError:
                continue
                           
            averagecolor = numpy.array([0.,0.,0.])
            
            # "pixels" has the structure [pixel1_red, pixel1_green, pixel1_blue, pixel1_alpha, pixel2_red, ...]
            samples = pixel_samples
            n = int(len(image.pixels)/4)
            # take no more samples than pixels exist
            if samples > n:
                samples = n

            pixels = image.pixels
            for i in range(samples):  
                pos = int(i/samples*n)*4
                averagecolor += image.pixels[pos:pos+3]
            
            averagecolor = averagecolor / samples   
            if True in numpy.isnan(averagecolor):  
                averagecolor[:] = [0,0,0]
            
            # adjust saturation and brightness (value)
            adjustedcolor = Color(averagecolor)
            hsv = list(colorsys.rgb_to_hsv(*adjustedcolor))
            hsv[1] += saturation - 1
            hsv[2] += value - 1
            new_rgb = colorsys.hsv_to_rgb(*hsv)
            # Colors may be out of range in some cases, norm to [0,1]
            if any(c>1 for c in new_rgb):
                max_rbg = max(new_rgb)
                new_rgb = (new_rgb[0]/max_rbg, 
                           new_rgb[1]/max_rbg, 
                           new_rgb[2]/max_rbg)
                too_bright = True
            material.diffuse_color = Color(new_rgb)
            material.diffuse_intensity = 1.0
            
            colors_calculated += 1
            if set_transparency: 
                material.use_transparency = True     
                
        self.report({'INFO'}, str(colors_calculated) + ' colors updated')   
        if too_bright:
            self.report({'INFO'}, str(colors_calculated) + ' colors updated (clamped)')   
        return {'FINISHED'}


# replaces all UV textures by the image that the material of this face has
class GothicAssignImages(bpy.types.Operator):
    """Apply UV-Images that correspond to the assigned materials"""      # blender will use this as a tooltip for menu items and buttons.
    bl_idname = "context.gothic_assign_images"        # unique identifier for buttons and menu items to reference.
    bl_label = "Gothic: Assign Images by Materials"         # display name in the interface.
    bl_options = {'REGISTER'}  # enable undo for the operator.

    def execute(self, context):        # execute() is called by blender when running the operator.

        scene = context.scene
        apply_to_selected_only = scene.gothic_tools.apply_to_selected_only

        if apply_to_selected_only:
            objects_tobechanged = context.selected_objects
            if not objects_tobechanged:
                self.report({'WARNING'}, 'No objects selected')
        else:
            objects_tobechanged = bpy.data.objects
            
        for object in objects_tobechanged:
            if not object.type == 'MESH':
                continue
                
            bpy.context.scene.objects.active = object
            bpy.ops.object.mode_set(mode = 'OBJECT')
            mesh = object.data
            if not mesh.uv_textures:
                uv = mesh.uv_textures.new('UvMap')
            # collect all materials and their iamge
            # map material index to image beforehands into dict: image_by_material_index
            image_by_material_index = [None]*len(object.material_slots)
            for matindex,matslot in enumerate(object.material_slots):
                # if texture or texture image doesn't exist, return None
                try:
                    image_by_material_index[matindex] = matslot.material.texture_slots[0].texture.image
                except AttributeError:
                    pass
            
            # assign image to face
            uv = object.data.uv_textures[0]
            for index,face in enumerate(mesh.polygons):
                uv.data[index].image = image_by_material_index[face.material_index]
                
        self.report({'INFO'}, 'UV-Images assigned to ' +str(len(objects_tobechanged)) + ' objects')        
        return {'FINISHED'} 


# replaces materials by those that belong to the assigned UV textures
# at every call matlib.ini is parsed and for every image a matching material is searched_material
# depending on how often this texture is used by a material, the used material name is
# never:    texture name without file extension
# once:     take name from materialfilter
# more:     ambiguous, depending on settings
# optionally faces with portal materials are not overwritten
# note that this will create a material for all used images in the file if they dont exist. this is done because
# it would be more troublesome to first filter out the actually needed materials
class GothicAssignMaterials(bpy.types.Operator):
    """Apply Materials that Correspond to the Unwrapped UV-Images"""      # blender will use this as a tooltip for menu items and buttons.
    bl_idname = "context.gothic_assign_materials"        # unique identifier for buttons and menu items to reference.
    bl_label = "Gothic: Assign Materials by UV-Images"         # display name in the interface.
    bl_options = {'REGISTER'}  # enable undo for the operator.

    def execute(self, context):        # execute() is called by blender when running the operator.
        
        scene = context.scene
        apply_to_selected_only = scene.gothic_tools.apply_to_selected_only
        matlib_filepath = scene.gothic_tools.matlib_filepath
        ambiguous_materials = scene.gothic_tools.ambiguous_materials
        matching_name = scene.gothic_tools.matching_name
        apply_to_selected_only = scene.gothic_tools.apply_to_selected_only 
        keep_portals = scene.gothic_tools.keep_portals
        # if no objects are selected and "only selected objects", cancel 
        if apply_to_selected_only:
            objects_tobechanged = context.selected_objects
            if not objects_tobechanged:
                self.report({'WARNING'}, 'No objects selected')
                return {'FINISHED'}
        # if no valid matlib.ini specified, cancel
        matlib_dirpath = os.path.dirname(matlib_filepath)
        if not os.path.isfile(matlib_filepath):
            self.report({'ERROR'}, 'Invalid MatLib.ini filepath')
            return {'CANCELLED'}  
        
        # for every used image create or find a matching texture
        # use existing textures with correct name if available
        # map image to texture into dict "texture_by_image"
        used_images = []
        texture_by_image = {}
        for image in bpy.data.images:
            if image.users:
                used_images.append(image)
                found_matching_texindex = bpy.data.textures.find(image.name)
                if found_matching_texindex == -1:
                    newtex = bpy.data.textures.new(image.name,'IMAGE')
                    newtex.image = image
                    texture_by_image[image] = newtex
                else:
                    texture_by_image[image] = bpy.data.textures[found_matching_texindex]

        
        """ gothic materials """
        # parse matlib
        # create one list of materials, one of corresponing textures and one for colors
        # same index for matching material/texture/color
        gmaterial_names = []
        gtexture_names = []
        gmaterial_colors = []
        
        # append found items to given input variables
        def add_materials_from_pml(file, materials, textures, colors):
            if not os.path.isfile(file):
                self.report({'WARNING'}, 'PML not found: ' + file)
                return
            file=open(file,'r')
            for line in file:
                if not line.find('% zCMaterial') == -1:
                    materials.append("")
                    textures.append("")
                    colors.append("")
                elif not line.find('name=string:') == -1:
                    materials[-1] = line[line.find('name=string:')+12:-1].upper()
                elif not line.find('texture=string:') == -1:
                    textures[-1] = line[line.find('texture=string:')+15:-1].upper()
                elif not line.find('color=color:') == -1:
                    colors[-1] = line[line.find('color=color:')+12:-1].split(' ')[:-1]
               
        matlib = open(matlib_filepath,'r')        
        for line in matlib: 
            if '=#' in line:
                add_materials_from_pml(os.path.join(matlib_dirpath, line[0:line.find('=#')]+'.pml'),gmaterial_names,gtexture_names, gmaterial_colors)
        
        # find materials that appear more than once
        # start from the end so that with duplicate materials
        # the lower index entry will be removed
        seenmaterials = set()
        duplicates = []
        for x in enumerate(list(reversed(gmaterial_names))):  
            if x[1] in seenmaterials:
                duplicates.append(len(gmaterial_names)-1-x[0])
            else:
                seenmaterials.add(x[1])
        # remove duplicate gothic materials from both lists
        for duplicate in duplicates:
            gmaterial_names.pop(duplicate)
            gtexture_names.pop(duplicate)
            
        # find gothic textures that are used by more than one material
        ambiguoustex_names = list(set([texname for texname in gtexture_names if gtexture_names.count(texname)>1]))
        ambiguoustex_defaultmat = {}
        for ambigtexname in ambiguoustex_names:
            # take first or last entry
            for index in range(len(gmaterial_names)):
                if gtexture_names == ambigtexname:
                    ambiguoustex_defaultmat[ambigtexname.lower()] = gmaterial_names[index]
                    # if first entry is taken: skip remaining 
                    # else defaultmat is overwritten every time
                    if ambiguous_materials=='first':
                        break   
            # if a material with same name exists and option checked, overwrite
            if matching_name:
                if ambigtexname in gmaterial_names:
                    ambiguoustex_defaultmat[ambigtexname.lower()] = ambigtexname
                # else if a material with same name except extension exists, take it as default
                elif ambigtexname[0:-4] in gmaterial_names:
                    ambiguoustex_defaultmat[ambigtexname.lower()] = ambigtexname[0:-4] 
                                        
        """ blender materials """
        # for every blender texture: what should be the material name
        # if no corresponding gtex: same name as in gothic
        # if one correspoding gtex: use the existing material name
        # if ambiguous: first, last or generic ('ambiguous1'...), additionally matching name if available
        # save the determined material name in var "bmat_name_by_image" mapped by image
        bmat_name_by_image = {}
        bmat_color_by_image = {}
        index_of_ambiguous = 1
        for image in used_images:
            gmat_exists = False      
            # gtex_index is used to find the gmat, because they have same indices
            for gtex_index, gtex_name in enumerate(gtexture_names):
                if gtex_name.lower() == image.name.lower():
                    bmat_color_by_image[image] = Color([int(x)/255 for x in gmaterial_colors[gtex_index]])           
                    if not gtex_name in ambiguoustex_names: 
                        bmat_name_by_image[image] = gmaterial_names[gtex_index]
                    else:
                        if ambiguous_materials=='generic':
                            bmat_name_by_image[image] = 'ambiguous'+str(index_of_ambiguous)
                            index_of_ambiguous += 1
                        else:
                            bmat_name_by_image[image]  = ambiguoustex_defaultmat[image.name.lower()]
                    
                    gmat_exists = True
                    break;
            if not gmat_exists:
                # take filename without extension and default color
                bmat_name_by_image[image] = os.path.basename(image.name).upper()[0:-4]
                bmat_color_by_image[image] = Color([0.8, 0.8, 0.8])

                
        # collect the materials that belong to any existing used image 
        # (not only those images that appear in the selected objects, because its simpled this way)
        # use existing materials with correct name if available
        # first create global 'unknown' material for faces without image
        # even if no unknown exist, zero users will still be a useful indicator
        if bpy.data.materials.find('unknown')==-1:
            matunknown = bpy.data.materials.new('unknown')
            matunknown.diffuse_color = Color([1,0,1])  # pink
        else:
            matunknown = bpy.data.materials[bpy.data.materials.find('unknown')]
        
        material_by_image = {}
        material_by_image[None] = matunknown
        for image,bmat_name in bmat_name_by_image.items():
            found_existing_bmat = False
            for scannedmaterial in bpy.data.materials:
                if scannedmaterial.name == bmat_name:
                    targetmat = scannedmaterial
                    found_existing_bmat = True
                    break;
            if not found_existing_bmat:
                targetmat = bpy.data.materials.new(bmat_name)
                targetmat.diffuse_color = bmat_color_by_image[image]
            material_by_image[image] = targetmat
            # determine texture for this material
            corresponding_texture = texture_by_image[image]
            for slot in  targetmat.texture_slots:
                if slot:
                    break
            else:
                targetmat.texture_slots.add()
            targetmat.texture_slots[0].texture = corresponding_texture

        # iterate over all polygons and look up the matching material
        # for every used image in the file the matching material is mapped inside var "material_by_image"
        if apply_to_selected_only:
            objects_tobechanged = context.selected_objects
        else:
            objects_tobechanged = bpy.data.objects
            
        for object in objects_tobechanged:
            if not object.type == 'MESH':
                continue
            if object.hide:
                continue
                
            bpy.context.scene.objects.active = object
            bpy.ops.object.mode_set(mode = 'OBJECT')
            
            mesh = object.data
            
            # keep_mat_with_index stores material slot numbers which will not be overwritten by UV (portals)     
            keep_mat_with_index = []
            # slot_is_used contains any material index that will not be deleted after reassigning the slots
            slot_is_used = [False]*len(object.material_slots)

            try:
                if keep_portals:
                    for matindex, matslot in enumerate(object.material_slots):
                        n = matslot.material.name.lower()
                        if n      == 'ghostoccluder' or \
                           n[0:2] == 'p:'  or \
                           n[0:3] == 'pi:' or \
                           n[0:3] == 'pn:':
                               keep_mat_with_index.append(matindex) 
                               slot_is_used[matindex] = True
            except AttributeError:
                pass
  
            if not mesh.uv_textures:
                # in this case only unknown material except portals will be assigned
                uv = mesh.uv_textures.new('UVMap')
            else:
                uv = mesh.uv_textures[0]

            # for every polygon look up which material matches its UV image    
            for index,face in enumerate(mesh.polygons):
                image = mesh.uv_textures[0].data[index].image
                # dont assign anything if not supposed to because its a portal
                if face.material_index in keep_mat_with_index:
                    continue;
                # if no image, take 'unknown' mat
                if not image:
                    mat = matunknown
                else:
                    # for every image a material should be mapped in material_by_image
                    if image in material_by_image:
                        mat = material_by_image[image]
                    else:
                        # something went wrong, most likely image users not updated correctly
                        raise ValueError('No mapped material found for '+image.name + '. Most likely the images are not updated internally. Try restarting Blender')
                        mat = matunknown
                
                # check if object has this material already
                for slotindex,slot in enumerate(object.material_slots):
                    if slot.material == mat:
                        face.material_index = slotindex
                        slot_is_used[slotindex] = True
                        break; 
                # if not, add a slot at bottom (new slot will be last)
                else:  
                    bpy.ops.object.material_slot_add()
                    object.active_material = mat
                    object.material_slots[object.active_material_index].link = 'DATA' 
                    face.material_index = object.active_material_index
                    slot_is_used.append(True)
            
            # delete unused slots from bottom to top
            for slot_reversed, used in enumerate(reversed(slot_is_used)):
                if not used:
                    slot = len(slot_is_used) - slot_reversed - 1
                    object.active_material_index = slot
                    bpy.ops.object.material_slot_remove()           

        self.report({'INFO'}, 'Materials assigned to ' +str(len(objects_tobechanged)) + ' objects')  
        return {'FINISHED'}            # this lets blender know the operator finished successfully.


# ------------------------------------------------------------------------
#    gothic tools in objectmode
# ------------------------------------------------------------------------

class VIEW3D_PT_gothic_clean_duplicates_panel(Panel):
    bl_idname = "OBJECT_PT_gothic_clean_duplicates_panel"
    bl_label = "Clean Duplicates"
    bl_space_type = "VIEW_3D"   
    bl_region_type = "TOOLS"    
    bl_category = "Gothic Materials"
    bl_context = "objectmode"   


    def draw(self, context):
        layout = self.layout
        scene = context.scene
        gothic_tools = scene.gothic_tools

        layout.operator('context.gothic_clean_images', text = 'Clean Images', icon = 'IMAGE_DATA')
        layout.operator('context.gothic_clean_materials', text = 'Clean Materials', icon = 'MATERIAL')
        
        layout.label(text="Case:")
        layout.prop(gothic_tools, "case", text="") 
        
        layout.separator()
        layout.separator()
        

class VIEW3D_PT_gothic_assign_materials_panel(Panel):
    bl_idname = "VIEW3D_PT_gothic_assign_materials_panel"
    bl_label = "UVs to Materials"
    bl_space_type = "VIEW_3D"   
    bl_region_type = "TOOLS"    
    bl_category = "Gothic Materials"
    bl_context = "objectmode"   

    def draw(self, context):
        layout = self.layout
        scene = context.scene
        gothic_tools = scene.gothic_tools
        
        layout.operator('context.gothic_assign_materials', text = 'Assign Materials', icon = 'FACESEL')
        
        layout.separator()
        
        layout.prop(gothic_tools, "matlib_filepath", text="")
        layout.prop(gothic_tools, "apply_to_selected_only")
        layout.prop(gothic_tools, "keep_portals")
        layout.separator()
        
        layout.label(text="Ambiguous Textures:")
        layout.prop(gothic_tools, "matching_name")
        layout.label(text="or else")
        layout.prop(gothic_tools, "ambiguous_materials", text="") 
        
        layout.separator()
        layout.separator()

        
class VIEW3D_PT_gothic_assign_images_panel(Panel):
    bl_idname = "VIEW3D_PT_gothic_assign_images_panel"
    bl_label = "Materials to UVs"
    bl_space_type = "VIEW_3D"   
    bl_region_type = "TOOLS"    
    bl_category = "Gothic Materials"
    bl_context = "objectmode"   

    def draw(self, context):
        layout = self.layout
        scene = context.scene
        gothic_tools = scene.gothic_tools
        
        layout.operator('context.gothic_assign_images', text = 'Assign Images', icon = 'FACESEL_HLT')
        layout.prop(gothic_tools, "apply_to_selected_only")
        
        layout.separator()
        layout.separator()
 
 
class VIEW3D_PT_gothic_material_colors_panel(Panel):
    bl_idname = "VIEW3D_PT_gothic_material_colors_panel"
    bl_label = "Material Colors"
    bl_space_type = "VIEW_3D"   
    bl_region_type = "TOOLS"    
    bl_category = "Gothic Materials"
    bl_context = "objectmode"   

    def draw(self, context):
        layout = self.layout
        scene = context.scene
        gothic_tools = context.scene.gothic_tools
        
        layout.operator('context.gothic_calc_colors', text = 'Calc Colors (slow)', icon = 'COLOR')
        row = layout.row()
        row.prop(gothic_tools, "set_transparency")
        row.prop(gothic_tools, "pixel_samples")
        layout.prop(gothic_tools, "saturation")
        layout.prop(gothic_tools, "value")
        
        layout.separator()
        layout.separator()


class VIEW3D_PT_gothic_search_material_panel(Panel):
    bl_idname = "VIEW3D_PT_gothic_search_material_panel"
    bl_label = "Search Material"
    bl_space_type = "VIEW_3D"   
    bl_region_type = "TOOLS"    
    bl_category = "Gothic Materials"
    bl_context = "objectmode"   

    def draw(self, context):
        layout = self.layout
        scene = context.scene
        gothic_tools = scene.gothic_tools
        
        layout.operator('object.gothic_isolate_objects', text = 'Isolate Objects', icon = 'VIEWZOOM')
        layout.prop(gothic_tools, "searched_material", text="with Mat")
        layout.prop(gothic_tools, "isolate_all_layers")
               
        layout.separator()
        layout.separator()        

        
# ------------------------------------------------------------------------
# register and unregister
# ------------------------------------------------------------------------

def register():
    bpy.utils.register_module(__name__)
    bpy.types.Scene.gothic_tools = PointerProperty(type=GothicMaterialSettings)

def unregister():
    bpy.utils.unregister_module(__name__)
    del bpy.types.Scene.gothic_tools

if __name__ == "__main__":
    register()