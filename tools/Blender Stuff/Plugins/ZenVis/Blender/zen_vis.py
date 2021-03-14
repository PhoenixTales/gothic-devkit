import bpy
import uuid
import os

def random_string():
    return str(uuid.uuid4()).upper().replace('-', '')

def prepare_scene():
    # randomize names and normalize filepath
    for img in bpy.data.images:
        img.name = random_string()
        img.filepath = os.path.splitext(os.path.basename(img.filepath))[0].upper() + '.TGA'
    for mat in bpy.data.materials:
        if (mat.texture_slots[0] is not None) and (mat.texture_slots[0].texture is not None) and (mat.texture_slots[0].texture.image is not None):
            rnd = random_string()
            mat.name = rnd
            mat.texture_slots[0].texture.name = rnd
            mat.texture_slots[0].texture.image.name = rnd + '.TGA'
        else:
            mat.name = mat.name.upper()

def clean_object(obj_name):
    # set a random material, texture and image name (to avoid Blender's auto renaming!)
    for slot in bpy.data.objects[obj_name].material_slots:
        try:
            mat = slot.material
            if (mat is None):
                continue
            if (mat.texture_slots[0] is not None) and (mat.texture_slots[0].texture is not None) and (mat.texture_slots[0].texture.image is not None):
                rnd = random_string()
                mat.name = rnd
                mat.texture_slots[0].texture.name = rnd
                mat.texture_slots[0].texture.image.name = rnd + '.TGA'
            else:
                mat.name = mat.name.upper()
        except Exception as e:
            print(str(e))

    # check all material slots and replace materials with existing ones if necessary
    for slot in bpy.data.objects[obj_name].material_slots:
        try:
            mat = slot.material
            if (mat is None):
                continue
            if (mat.texture_slots[0] is not None) and (mat.texture_slots[0].texture is not None) and (mat.texture_slots[0].texture.image is not None):
                name = os.path.splitext(os.path.basename(mat.texture_slots[0].texture.image.filepath))[0].upper()
                if name in bpy.data.materials:
                    # replace material if another with the same name exists
                    slot.material = bpy.data.materials[name]
                    mat.user_clear()
                else:
                    # otherwise this is the first of it's kind, we should clean it
                    mat.name = name
                    mat.specular_intensity = 0.0
                    mat.texture_slots[0].texture.name = name
                    mat.texture_slots[0].texture.image.name = name + '.TGA'
                    mat.texture_slots[0].texture.image.filepath = name + '.TGA'
                    for i in range(1, len(mat.texture_slots)):
                        mat.texture_slots.clear(i)
            else:
                split = mat.name.rpartition('.')
                if split[2].isnumeric() and split[0] in bpy.data.materials:
                    # replace material if another with the same name exists
                    slot.material = bpy.data.materials[split[0]]
        except Exception as e:
            print(str(e))

    # if there is a material in multiple slots, remap polys and remove the slot
    mats = dict()
    for i in reversed(range(len(bpy.data.objects[obj_name].material_slots))):
        try:
            mat = bpy.data.objects[obj_name].material_slots[i].material
            if (mat is None):
                continue
            if mat.name not in mats:
                mats[mat.name] = i
            else:
                # select all polys with the old material
                for poly in bpy.data.objects[obj_name].data.polygons:
                    poly.select = (poly.material_index == i)
                # assign the new material
                bpy.context.scene.objects.active = bpy.data.objects[obj_name]
                bpy.data.objects[obj_name].active_material_index = mats[mat.name]
                bpy.ops.object.material_slot_assign()
                # deselect all polys
                for poly in bpy.data.objects[obj_name].data.polygons:
                    poly.select = False
                # remove the material slot
                bpy.context.scene.objects.active = bpy.data.objects[obj_name]
                bpy.data.objects[obj_name].active_material_index = i
                bpy.ops.object.material_slot_remove()
        except Exception as e:
            print(str(e))

    # assign the correct image to all polys (they have a material_index, however this is necessary for some obscure reason!)
    for uvtex in bpy.data.objects[obj_name].data.uv_textures:
        for poly in uvtex.data:
            try:
                if (poly.image is not None):
                    name = os.path.splitext(os.path.basename(poly.image.filepath))[0].upper() + '.TGA'
                    poly.image = bpy.data.images[name]
            except Exception as e:
                print(str(e))

def remove_unused_images():
    # create a set of all image names in use
    imgs = set()
    for mat in bpy.data.materials:
        if mat.users and (mat.texture_slots[0] is not None) and (mat.texture_slots[0].texture is not None) and (mat.texture_slots[0].texture.image is not None):
            imgs.add(mat.texture_slots[0].texture.image.name)

    # remove images by force (the user count is outdated but I couldn't figure out how to trigger recalculation!)
    for img in bpy.data.images:
        if img.name not in imgs:
            img.user_clear()
            bpy.data.images.remove(img)
        else:
            name = os.path.splitext(os.path.basename(img.filepath))[0].upper()
            img.filepath = name + '.TGA'