# image_search.py: provides the "load_image" function which can search an image file using some directory list.
# This file also provides GUI to setup the list of the texture directories.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------


# Import the required modules
import bpy
from bpy.props import *
import os


#--------------------------------------------------------------------------------------------------------------------------------
# Global variables and the "load_image" function
#--------------------------------------------------------------------------------------------------------------------------------
 
class TextureDirectory(bpy.types.PropertyGroup):
	pass

bpy.utils.register_class(TextureDirectory)

bpy.types.Scene.texture_directories = CollectionProperty( name = "Texture directories", type = TextureDirectory, description= "" )
bpy.types.Scene.selected_texture_directory = IntProperty( name= "Selected texture directory", default = -1, min = -1)

# Load image with searching in directories specified by the "paths" global variable
def load_image(filepath):
	if filepath == "":
		return None
	
	found = False
	filename = os.path.basename(filepath)
	
	i = bpy.data.images.find(filename)
	if i != -1:
		return bpy.data.images[i]
	
	if os.path.exists(filepath):
		found = True
	else:
		sce = bpy.context.scene
		texdirs = sce.texture_directories
		if(len(texdirs) == 0):
			load_paths()
		for texdir in texdirs:
			filepath = os.path.join(texdir.name, filename)
			if os.path.exists(filepath):
				found = True
				break
	
	if found:
		im = bpy.data.images.load(filepath)
	else:
		im = bpy.data.images.new(filename, 4, 4)
		im.source = "FILE"
		im.filepath = filename
	return im


	
#--------------------------------------------------------------------------------------------------------------------------------
# Operations on texture directories
#--------------------------------------------------------------------------------------------------------------------------------

# Function: Adds a path
def add_path(path):
	# nothing to do it the path is empty
	if(path == ""):
		return

	# remove filename from the selected path
	path2 = path
	if(path2[-1] != os.sep and path2[-1] != os.altsep):
		i = max(path2.rfind(os.sep), path2.rfind(os.altsep))
		if(i != -1):
			path2 = path2[:i+1]
		
	# append the new path to the list if the list doesn't contain it yet
	sce = bpy.context.scene
	texdirs = sce.texture_directories
	if(texdirs.get(path2) == None):
		newindex = len(texdirs)
		texdirs.add()
		texdirs[newindex].name = path2
		sce.selected_texture_directory = newindex
			
# Function: Adds a path with its subpaths
def add_path_with_subpaths(path):
	# nothing to do it the path is empty
	if(path == ""):
		return

	# remove filename from the selected path
	path2 = path
	if(path2[-1] != os.sep and path2[-1] != os.altsep):
		i = max(path2.rfind(os.sep), path2.rfind(os.altsep))
		if(i != -1):
			path2 = path2[:i+1]
	
	# append the new dirpath to the list if the list doesn't contain it yet
	sce = bpy.context.scene
	texdirs = sce.texture_directories
	for root, dirs, files in os.walk(path2):
		subpath = root.rstrip(os.sep + os.altsep) + os.sep
		if(texdirs.get(subpath) == None):
			add_path(subpath)

# Function: Moves a path upward (exchange a specified path with the previous one)
def move_path_up(index):
	sce = bpy.context.scene
	texdirs = sce.texture_directories
	if sce.selected_texture_directory >= 1:
		texdirs.move(sce.selected_texture_directory - 1, sce.selected_texture_directory)
		sce.selected_texture_directory -= 1
			
# Function: Moves a path downward (exchange a specified path with the next one)
def move_path_down(index):
	sce = bpy.context.scene
	texdirs = sce.texture_directories
	if sce.selected_texture_directory >= 0 and sce.selected_texture_directory <= len(texdirs) - 2:
		texdirs.move(sce.selected_texture_directory, sce.selected_texture_directory + 1)
		sce.selected_texture_directory += 1	
			
# Function: Removes a path by index (0...num-1)
def remove_path(index):
	sce = bpy.context.scene
	texdirs = sce.texture_directories
	texdirs.remove(index)
	if(sce.selected_texture_directory >= len(texdirs)):
		sce.selected_texture_directory = len(texdirs) - 1

# Function: Removes all the paths
def remove_all_paths():
	sce = bpy.context.scene
	texdirs = sce.texture_directories
	while(len(texdirs) > 0):
		texdirs.remove(0)
	if sce.selected_texture_directory != -1:
		sce.selected_texture_directory = -1
	
#  Function: Gets path to configuration file
def get_config_filepath():
	return bpy.utils.user_resource("CONFIG").rstrip(os.sep + os.altsep) + os.sep + "TextureDirs.cfg"

# Function: Saves paths to configuration file
def save_paths():
	cfgfilepath = get_config_filepath()
	cfgfile = None
	try:
		cfgfile = open(cfgfilepath, "wt")
		sce = bpy.context.scene
		texdirs = sce.texture_directories
		for texdir in texdirs:
			cfgfile.write(texdir.name)
			cfgfile.write("\n")
	except:
		pass
	finally:
		if(cfgfile != None):
			cfgfile.close()	
			
# Function: Loads paths to configuration file
def load_paths():
	remove_all_paths()
	cfgfilepath = get_config_filepath()
	cfgfile = None
	try:
		cfgfile = open(cfgfilepath, "rt")
		for line in cfgfile:
			if(line[-1] == "\n"):
				line = line[:-1]
				add_path(line)
	except:
		pass
	finally:
		if(cfgfile != None):
			cfgfile.close()	




#--------------------------------------------------------------------------------------------------------------------------------
# GUI
#--------------------------------------------------------------------------------------------------------------------------------

# Button "Add a path"
class TEXDIRS_OT_add_path(bpy.types.Operator):
	bl_idname	  = 'texdirs.add_path'
	bl_label	   = "Select path"
	
	# This property is used by the file browser
	filepath = StringProperty(name = "File Path")
	
	def invoke(self, context, event):
		wm = bpy.context.window_manager
		# show the file browser
		wm.fileselect_add(self)
		return {'RUNNING_MODAL'}
		
	def execute(self, context):
		add_path(self.filepath)
		save_paths()
		return {'FINISHED'}

# Button "Add path with subpaths"
class TEXDIRS_OT_add_path_with_subpaths(bpy.types.Operator):
	bl_idname	  = 'texdirs.add_path_with_subpaths'
	bl_label	   = "Select path"

	# This property is used by the file browser
	filepath = StringProperty(name = "File Path")
	
	def invoke(self, context, event):
		wm = bpy.context.window_manager
		# show the file browser
		wm.fileselect_add(self)
		return {'RUNNING_MODAL'}
		
	def execute(self, context):
		add_path_with_subpaths(self.filepath)
		save_paths()
		return {'FINISHED'}
		
# Button "Move up"
class TEXDIRS_OT_move_up(bpy.types.Operator):
	bl_idname	  = 'texdirs.move_up'
	bl_label	   = "Move up"
 
	def invoke(self, context, event):
		sce = bpy.context.scene
		move_path_up(sce.selected_texture_directory)
		save_paths()
		return{'FINISHED'}

# Button "Move down"
class TEXDIRS_OT_move_down(bpy.types.Operator):
	bl_idname	  = 'texdirs.move_down'
	bl_label	   = "Move down"
 
	def invoke(self, context, event):
		sce = bpy.context.scene
		move_path_down(sce.selected_texture_directory)
		save_paths()
		return{'FINISHED'}
		
# Button "Remove path"
class TEXDIRS_OT_remove_path(bpy.types.Operator):
	bl_idname	  = 'texdirs.remove_path'
	bl_label	   = "Remove path"
 
	def invoke(self, context, event):
		sce = bpy.context.scene
		remove_path(sce.selected_texture_directory)
		save_paths()
		return{'FINISHED'}
 
# Button "Remove all"
class TEXDIRS_OT_remove_all_paths(bpy.types.Operator):
	bl_idname	  = 'texdirs.remove_all_paths'
	bl_label	   = "Remove all paths"

	def invoke(self, context, event):
		remove_all_paths()
		save_paths()
		return{'FINISHED'}
 
class TEXDIRS_UI_list_slot(bpy.types.UIList):
	def draw_item(self, context, layout, data, item, icon, active_data, active_propname, index):
		layout.label(text=item.name)

# A new panel in the user preferences
class TextureDirectoriesPanel(bpy.types.Panel):
	bl_space_type  = 'USER_PREFERENCES'
	bl_region_type = 'WINDOW'
	bl_label = "List of texture directories (used by the KrxImpExp scripts)"
	
	def __init__(self):
		sce = bpy.context.scene
		texdirs = sce.texture_directories
		if(len(texdirs) == 0):
			load_paths()

	def draw(self, context):
		layout= self.layout
		sce = bpy.context.scene
		texdirs = sce.texture_directories
		
		row0 = layout.row()
		row0.template_list('TEXDIRS_UI_list_slot', '', sce, 'texture_directories', sce, 'selected_texture_directory', rows = 5, maxrows = 7)
		col = row0.column(align=True)
		col.scale_x = 0.35
		col.operator('texdirs.add_path', text="Add path")
		col.operator('texdirs.add_path_with_subpaths', text="Add path with subpaths")
		col.operator('texdirs.move_up', text="Move up")
		col.operator('texdirs.move_down', text="Move down")
		col.operator('texdirs.remove_path', text="Remove path")
		col.operator('texdirs.remove_all_paths', text="Remove all paths")
		row1 = layout.row()
		
		t = "Total: " + str(len(texdirs))
		index = sce.selected_texture_directory
		if(0 <= index and index < len(texdirs)):
			t += "			 "
			t += "Selected: " + str(index+1)
			t += "   "
			t += "'" + texdirs[index].name + "'"
		row1.label(text = t)

def register():
	bpy.utils.register_class(TextureDirectoriesPanel)
	bpy.utils.register_class(TEXDIRS_UI_list_slot)
	bpy.utils.register_class(TEXDIRS_OT_add_path)
	bpy.utils.register_class(TEXDIRS_OT_add_path_with_subpaths)
	bpy.utils.register_class(TEXDIRS_OT_move_up)
	bpy.utils.register_class(TEXDIRS_OT_move_down)
	bpy.utils.register_class(TEXDIRS_OT_remove_path)
	bpy.utils.register_class(TEXDIRS_OT_remove_all_paths)
	
def unregister():
	bpy.utils.unregister_class(TextureDirectoriesPanel)
	bpy.utils.unregister_class(TEXDIRS_UI_list_slot)
	bpy.utils.unregister_class(TEXDIRS_OT_add_path)
	bpy.utils.unregister_class(TEXDIRS_OT_add_path_with_subpaths)
	bpy.utils.unregister_class(TEXDIRS_OT_move_up)
	bpy.utils.unregister_class(TEXDIRS_OT_move_down)
	bpy.utils.unregister_class(TEXDIRS_OT_remove_path)
	bpy.utils.unregister_class(TEXDIRS_OT_remove_all_paths)
	
if __name__ == "__main__":
	register()
