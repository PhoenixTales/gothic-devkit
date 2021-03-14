# scene.py: Scene utilities.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------


# Import required modules
import bpy
import os

# Get the current scene's filename (which ends with ".blend"), without path.
def get_scene_filename():
	s = bpy.data.filepath
	i = max(s.rfind(os.sep), s.rfind(os.altsep))
	if i != -1:
		s = s[i+1:]
	return s

# Reset the current scene
def reset_scene():
	# Remove scenes
	empty_scn = bpy.data.scenes.new("Scene")
	for i in range(len(bpy.data.scenes) - 1, -1, -1):
		scn = bpy.data.scenes[i]
		if scn != empty_scn:
			bpy.data.scenes.remove(scn, do_unlink=True)
	empty_scn.name = 'Scene'
	
	# Remove objects
	for i in range(len(bpy.data.objects) - 1, -1, -1):
		obj = bpy.data.objects[i]
		if obj.users == 0:
			bpy.data.objects.remove(obj)
	
	# Remove meshes
	for i in range(len(bpy.data.meshes) - 1, -1, -1):
		msh = bpy.data.meshes[i]
		if msh.users == 0:
			bpy.data.meshes.remove(msh)
	
	# Remove armatures
	for i in range(len(bpy.data.armatures) - 1, -1, -1):
		arm = bpy.data.armatures[i]
		if arm.users == 0:
			bpy.data.armatures.remove(arm)
	
	# Remove actions
	for i in range(len(bpy.data.actions) - 1, -1, -1):
		act = bpy.data.actions[i]
		if act.users == 0:
			bpy.data.actions.remove(act)
	
	# Remove materials
	for i in range(len(bpy.data.materials) - 1, -1, -1):
		mat = bpy.data.materials[i]
		if mat.users == 0:
			bpy.data.materials.remove(mat)
	
	# Remove textures
	for i in range(len(bpy.data.textures) - 1, -1, -1):
		tex = bpy.data.textures[i]
		if tex.users == 0:
			bpy.data.textures.remove(tex)
	
	# Remove images
	for i in range(len(bpy.data.images) - 1, -1, -1):
		img = bpy.data.images[i]
		if img.users == 0:
			bpy.data.images.remove(img)
	
	
# Return path to the addons' directory, with trailing separator.
def get_root_dir():
	s = bpy.utils.script_paths("addons", False)[0] + os.sep
	return s
	
# Returns path to directory with configuration files, with trailing separator.
def get_plugcfg_dir():
	s = bpy.utils.user_resource("CONFIG")
	s += os.sep
	return s

