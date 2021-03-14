# animation.py: Utilities to work with animation
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------

# Import required modules
import bpy
from KrxImpExp.math_utils import *
from KrxImpExp.bone_utils import *


#--------------------------------
# General properties            #
#--------------------------------

# Returns the minimal frame index, which is possible to set in Blender.
def get_min_frame():
	return 0
	
# Returns the maximum frame index, which is possible to set in Blender.
def get_max_frame():
	return 300000

# Returns the start frame index, which is set in the current scene. 
def get_start_frame():
	scene = bpy.context.scene
	return scene.frame_start

# Returns the end frame index, which is set in the current scene. 
def get_end_frame():
	scene = bpy.context.scene
	return scene.frame_end

# Sets the start and end frame indices to be used in the current scene. 
def set_frame_range(start, end):
	scene = bpy.context.scene
	scene.frame_start = start
	scene.frame_end = end

# Returns FPS.
def get_fps():
	scene = bpy.context.scene
	return scene.render.fps

# Sets FPS
def set_fps(rate):
	scene = bpy.context.scene
	scene.render.fps = rate

# Returns the current frame.
def get_current_frame():
	scene = bpy.context.scene
	return scene.frame_current

# Sets the current frame.
def set_current_frame(t):
	scene = bpy.context.scene
	scene.frame_set(t)

	
#-----------------------------------------------
# Animation of object's transformation matrix  #
#-----------------------------------------------

def has_tm_animation(extobj):
	if type(extobj) == bpy.types.Object:
		animdata = extobj.animation_data
		if animdata != None:
			action = animdata.action
			if action != None:
				curvepath_pos = "location"
				curvepath_rot = "rotation_euler"
				curvepath_scale = "rotation_euler"
				for fc in action.fcurves:
					if fc.data_path == curvepath_pos or fc.data_path == curvepath_rot or fc.data_path == curvepath_scale:
						return True
		return False
		
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		armobj = extobj[0]
		bone_name = extobj[1]
		animdata = armobj.animation_data
		if animdata != None:
			action = animdata.action
			if action != None:
				curvepath_pos = "pose.bones[\"" + bone_name + "\"].location"
				curvepath_rot = "pose.bones[\"" + bone_name + "\"].rotation_quaternion"
				curvepath_scale = "pose.bones[\"" + bone_name + "\"].scale"
				for fc in action.fcurves:
					if fc.data_path == curvepath_pos or fc.data_path == curvepath_rot or fc.data_path == curvepath_scale:
						return True
		return False
		
	else:
		raise TypeError("delete_tm_animation: error with argument 1")	

def delete_tm_animation(extobj):
	if type(extobj) == bpy.types.Object:
		animdata = extobj.animation_data
		if animdata != None:
			action = animdata.action
			if action != None:
				curvepath_pos = "location"
				curvepath_rot = "rotation_euler"
				curvepath_scale = "rotation_euler"
				for fc in action.fcurves:
					if fc.data_path == curvepath_pos or fc.data_path == curvepath_rot or fc.data_path == curvepath_scale:
						action.fcurves.remove(fc)
		
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		armobj = extobj[0]
		bone_name = extobj[1]
		animdata = armobj.animation_data
		if animdata != None:
			action = animdata.action
			if action != None:
				curvepath_pos = "pose.bones[\"" + bone_name + "\"].location"
				curvepath_rot = "pose.bones[\"" + bone_name + "\"].rotation_quaternion"
				curvepath_scale = "pose.bones[\"" + bone_name + "\"].scale"
				for fc in action.fcurves:
					if fc.data_path == curvepath_pos or fc.data_path == curvepath_rot or fc.data_path == curvepath_scale:
						action.fcurves.remove(fc)
	else:
		raise TypeError("delete_tm_animation: error with argument 1")	

def animate_tm(extobj, newtransform):
	frame = bpy.context.scene.frame_current
	if type(extobj) == bpy.types.Object:
		new_tm = unprepare_matrix(newtransform)
		parent = extobj.parent
		if parent != None and extobj.parent_type == "BONE":
			arm_obj = extobj.parent
			arm_mat = arm_obj.matrix_world
			arm = arm_obj.data
			new_obj_mat = arm_mat.inverted() * new_tm
			parent_bone_name = extobj.parent_bone
			parent_bone_pose_mat = parent.pose.bones[parent_bone_name].matrix
			parent_bone_arm_mat = arm.bones[parent_bone_name].matrix_local
			m = new_obj_mat
			pa = parent_bone_arm_mat
			pmi = parent_bone_pose_mat.inverted()
			final_mat = pa * (pmi * m)
		else:
			final_mat = new_tm
		
		pos = final_mat.to_translation()
		rot = final_mat.to_euler()
		
		animdata = extobj.animation_data
		if animdata == None:
			animdata = extobj.animation_data_create()
		action = animdata.action
		if action == None:
			action = animdata.action = bpy.data.actions.new(extobj.name + "Action")
		
		curvepath_pos = "location"
		curvepath_rot = "rotation_euler"
		curve_pos = [None, None, None]
		curve_rot = [None, None, None]
		for fc in action.fcurves:
			if fc.data_path == curvepath_pos:
				curve_pos[fc.array_index] = fc
			elif fc.data_path == curvepath_rot:
				curve_rot[fc.array_index] = fc
		
		for i in range(0, 3):
			if curve_pos[i] == None:
				curve_pos[i] = action.fcurves.new(curvepath_pos, i, "LocRotScale")
			if curve_rot[i] == None:
				curve_rot[i] = action.fcurves.new(curvepath_rot, i, "LocRotScale")
			curve_pos[i].keyframe_points.insert(frame, pos[i])
			curve_rot[i].keyframe_points.insert(frame, rot[i])
	
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		new_tm = unprepare_bone_matrix(newtransform)
		arm_obj = extobj[0]
		arm = arm_obj.data
		bone_name = extobj[1]
		
		arm_mat = arm_obj.matrix_world
		new_bone_mat = arm_mat.inverted() * new_tm
		
		arm_bone = arm.bones[bone_name]
		bone_arm_mat = arm_bone.matrix_local
		if arm_bone.parent != None:
			parent_bone_arm_mat = arm_bone.parent.matrix_local
		else:
			parent_bone_arm_mat = Matrix.Identity(4)
		
		bone_pose_mat = new_bone_mat
		pose_bone = arm_obj.pose.bones[bone_name]
		if pose_bone.parent != None:
			parent_bone_pose_mat = pose_bone.parent.matrix
		else:
			parent_bone_pose_mat = Matrix.Identity(4)
		
		m = bone_pose_mat
		pmi = parent_bone_pose_mat.inverted()
		pa = parent_bone_arm_mat
		ai = bone_arm_mat.inverted()
		final_mat = (ai * pa) * (pmi * m)
		
		pos  = final_mat.to_translation()
		rot  = final_mat.to_quaternion()
		
		animdata = arm_obj.animation_data
		if animdata == None:
			animdata = arm_obj.animation_data_create()
		action = animdata.action
		if action == None:
			action = animdata.action = bpy.data.actions.new(arm_obj.name + "Action")
		
		curvepath_pos = "pose.bones[\"" + bone_name + "\"].location"
		curvepath_rot = "pose.bones[\"" + bone_name + "\"].rotation_quaternion"
		curve_pos = [None, None, None]
		curve_rot = [None, None, None, None]
		for fc in action.fcurves:
			if fc.data_path == curvepath_pos:
				curve_pos[fc.array_index] = fc
			elif fc.data_path == curvepath_rot:
				curve_rot[fc.array_index] = fc
		
		for i in range(0, 3):
			if curve_pos[i] == None:
				curve_pos[i] = action.fcurves.new(curvepath_pos, i, bone_name)
			curve_pos[i].keyframe_points.insert(frame, pos[i])
		for i in range(0, 4):
			if curve_rot[i] == None:
				curve_rot[i] = action.fcurves.new(curvepath_rot, i, bone_name)
			curve_rot[i].keyframe_points.insert(frame, rot[i])
	else:
		raise TypeError("animate_tm: error with argument 1")	


#--------------------------------------------------
# Animation of mesh's vertices (morph animation)  #
#--------------------------------------------------

def has_vertex_animation(extobj):
	if type(extobj) == bpy.types.Object and extobj.type == "MESH":
		msh = extobj.data
		if msh != None:
			if msh.shape_keys != None:
				return True
	return False

def delete_vertex_animation(extobj):
	if type(extobj) == bpy.types.Object and extobj.type == "MESH":
		msh = extobj.data
		if msh != None:
			bpy.context.scene.objects.active = extobj
			while msh.shape_keys != None:
				bpy.ops.object.shape_key_remove()

def animate_vertices(extobj, points):
	frame = bpy.context.scene.frame_current
	if type(extobj) == bpy.types.Object and extobj.type == "MESH":
		msh = extobj.data
		if msh != None:
			bpy.context.scene.objects.active = extobj
			bpy.ops.object.shape_key_add()
			key = msh.shape_keys
			key.use_relative = True
			
			curshapekey = key.key_blocks[-1]
			for i in range(0, len(points)):
				pt = points[i]
				curshapekey.data[i].co = pt
			
			animdata = key.animation_data
			if animdata == None:
				animdata = key.animation_data_create()
			action = animdata.action
			if action == None:
				action = animdata.action = bpy.data.actions.new(extobj.name + "Action")
			
			for shapekey in key.key_blocks:
				curvepath = "key_blocks[\"" + shapekey.name + "\"].value"
				curve = None
				for fc in action.fcurves:
					if fc.data_path == curvepath:
						curve = fc
						break
				
				if shapekey == curshapekey:
					if curve == None:
						curve = action.fcurves.new(curvepath)
					if frame != get_min_frame():
						curve.keyframe_points.insert(frame - 1, 0.0)
					curve.keyframe_points.insert(frame, 1.0)
					if frame != get_max_frame():
						curve.keyframe_points.insert(frame + 1, 0.0)
				else:
					if curve != None:
						for kp in curve.keyframe_points:
							if kp.co[0] == frame:
								kp.co[1] = 0
