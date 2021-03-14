# bone.py: Creation of bones.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------

# Import required modules
import bpy
from KrxImpExp.object import *
from KrxImpExp.math_utils import *
from KrxImpExp.bone_utils import *

# Creates a new bone.
# Parameters:
# name - name of the new bone
# extparent - None or tuple (object_with_armature_data, "parent bone's name")
# bound_box_min, bound_box_max - bounding box for a new bone, used for displaying the new bone only.
# The function returns an extended object (see note in object_utils.py).
def new_bone_object(name, extparent, bound_box_min, bound_box_max, skintype = "Armature modifier"):
	(obj_name, bone_name) = split_bone_name(name)
	
	# Create an armature if it has not been created yet
	if extparent == None:
		arm = bpy.data.armatures.get(obj_name)
		if arm == None:
			arm = bpy.data.armatures.new(obj_name)
			arm.draw_type = "STICK"
		obj = bpy.data.objects.get(obj_name)
		if obj == None:
			obj = bpy.data.objects.new(obj_name, object_data = arm)
			obj.show_x_ray = True
			bpy.context.scene.objects.link(obj)
		parent_bone_name = ""
	elif type(extparent) == tuple and len(extparent) == 2 and type(extparent[0]) == bpy.types.Object and type(extparent[1]) == str:
		obj = extparent[0]
		arm = obj.data
		parent_bone_name = extparent[1]
	else:
		raise TypeError("new_bone_object: error with argument 2")
	
	# Switch to edit mode
	start_editmode(obj)
	
	# Create a new bone
	eb = arm.edit_bones.new(bone_name)
	real_name = eb.name
	if parent_bone_name != "":
		eb.parent = arm.edit_bones[parent_bone_name]
	eb.head = Vector((bound_box_min.x, 0, 0))
	eb.tail = Vector((bound_box_max.x, 0, 0))
	eb.roll = 0
	
	# Switch back to object mode
	end_editmode()
	
	# Return a tuple with information about the new bone
	return ((obj, real_name))

	
