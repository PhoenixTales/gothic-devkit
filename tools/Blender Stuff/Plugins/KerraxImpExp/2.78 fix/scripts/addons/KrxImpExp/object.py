# object.py: Object utilities.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------

# Import required modules
import bpy
from KrxImpExp.bone_utils import *
from KrxImpExp.math_utils import *

# Note:
# Some of the functions below get an argument named "extended object", (or "extobj", or "extparent", and so on)
# The "Extended object" may be one of: 
# a) None means the root object
# b) value of type bpy.types.Object means any Blender's object
# c) (object, bone_name), i.e. a tuple of two values, the first is an object which contains an armature,
#    and the second is a bone's name

# Returns name of an extended object
def get_object_name(extobj):
	if type(extobj) == bpy.types.Object:
		return extobj.name
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		return make_bone_name(obj.name, bone_name) # Armature object's name + bone's name
	else:
		raise TypeError("get_object_name: error with argument 1")

# Changes name of an extended object
def set_object_name(extobj, newname):
	if type(extobj) == bpy.types.Object:
		extobj.name = newname
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		(new_obj_name, new_bone_name) = split_bone_name(name)
		
		obj.name = new_obj_name
		arm = obj.data
		arm.name = new_obj_name
		
		start_editmode(obj)
		arm.edit_bones[bone_name].name = new_bone_name
		end_editmode(obj)
	else:
		raise TypeError("set_object_name: error with argument 1")

# Returns (extended) parent of an extended object
def get_parent(extobj):
	if type(extobj) == bpy.types.Object:
		if extobj.parent_type == "BONE":
			return ((extobj.parent, extobj.parent_bone)) # A normal object linked to a bone
		else:
			return extobj.parent # A normal object linked to another object
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		arm = obj.data
		bone_name = extobj[1]
		bone = arm.bones[bone_name]
		if bone.parent == None:
			return obj # The root bone is considered to be linked to armature object
		else:
			return ((obj, bone.parent.name)) # Bone is linked to another bone
	else:
		raise TypeError("get_parent_object: error with argument 1")

# Returns list of child objects for an extended object
def get_children(extobj):
	children = []
	if extobj == None:
		# children of the root object = objects with no parent	
		for o in bpy.context.scene.objects:
			if o.parent == None:
				children.append(o)
	elif type(extobj) == bpy.types.Object:
		if extobj.type == "ARMATURE": 
			# children of armature object are bones with no parent
			arm = extobj.data
			for b in arm.bones:
				if b.parent == None:
					children.append((extobj, b.name))
		# children of object
		for o in bpy.context.scene.objects:
			if o.parent == extobj and o.parent_type == "OBJECT":
				children.append(o)
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		# children of bone
		obj = extobj[0]
		arm = obj.data
		bone_name = extobj[1]
		for b in arm.bones:
			if b.parent != None and b.parent.name == bone_name:
				children.append((obj, b.name))
		for o in bpy.context.scene.objects:
			if o.parent == obj and o.parent_type == "BONE" and o.parent_bone == bone_name:
				children.append(o)
	else:
		raise TypeError("get_child_objects: error with argument 1")
		
	return children

# Changes parent of an extended object
def set_parent(extobj, extnewparent):
	if type(extobj) == bpy.types.Object:
		# clear the previous parent object
		deselect_all()
		select(extobj)
		bpy.ops.object.parent_clear(type = "CLEAR_KEEP_TRANSFORM")
		
		# set new parent
		if extnewparent == None:
			pass
		elif type(extnewparent) == bpy.types.Object:
			bpy.context.scene.objects.active = extnewparent
			bpy.ops.object.parent_set(type = "OBJECT")
		elif type(extnewparent) == tuple and len(extnewparent) == 2 and type(extnewparent[0]) == bpy.types.Object and type(extnewparent[1]) == str:
			newparent = extnewparent[0]
			arm = newparent.data
			parent_bone_name = extnewparent[1]
			arm.bones.active = arm.bones[parent_bone_name]
			bpy.context.scene.objects.active = newparent
			bpy.ops.object.parent_set(type = "BONE")
		else:
			raise TypeError("set_parent_object: error with argument 2")
	
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		arm = obj.data
		bone_name = extobj[1]
		parent_bone_name = extnewparent[1]
		start_editmode(obj)
		eb = arm.edit_bones[bone_name]
		eb.parent = None
		end_editmode()
		
		if extnewparent == None:
			pass
		elif type(extnewparent) == tuple and len(extnewparent) == 2 and type(extnewparent[0]) == bpy.types.Object and type(extnewparent[1]) == str and extnewparent[0] == obj:
			parent_bone_name = extnewparent[1]
			start_editmode(obj)
			eb = arm.edit_bones[bone_name]
			eb.parent = arm.edit_bones[parent_bone_name]
			end_editmode()
		else:
			raise TypeError("set_parent_object: error with argument 2")
	
	else:
		raise TypeError("clear_parent_object: error with argument 1")
		
# Returns an extended object's transformation matrix (4x4)
def get_transform(extobj):
	if type(extobj) == bpy.types.Object:
		return prepare_matrix(extobj.matrix_world)
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		arm_obj = extobj[0]
		bone_name = extobj[1]
		pose = arm_obj.pose
		pose_mat = pose.bones[bone_name].matrix
		arm_mat = arm_obj.matrix_world
		cur_transform = arm_mat * pose_mat
		return prepare_bone_matrix(cur_transform)
	else:
		raise TypeError("get_object_transform: error with argument 1")

# Changes an extended object's transformation matrix (4x4) - don't use it to create animation
def set_transform(extobj, newtransform):
	if type(extobj) == bpy.types.Object:
		# not a bone
		new_tm = unprepare_matrix(newtransform)
		extobj.matrix_basis = new_tm
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		# bone
		new_tm = unprepare_bone_matrix(newtransform)
		arm_obj = extobj[0]
		arm = arm_obj.data
		bone_name = extobj[1]
		arm_mat = arm_obj.matrix_world
		new_bone_mat = arm_mat.inverted() * new_tm
		# if some mesh is linked to this armature, set pose; else edit armature's bone itself.
		noAttachedMesh = True
		for o in bpy.context.scene.objects:
			if o.parent == arm_obj:
				noAttachedMesh = False
				break
		if noAttachedMesh:
			start_editmode(arm_obj)
			boneO = new_bone_mat.col[3].to_3d()
			boneY = new_bone_mat.col[1].to_3d()
			boneZ = new_bone_mat.col[2].to_3d()
			eb = arm.edit_bones[bone_name]
			length = eb.length
			eb.head = boneO
			eb.tail = boneO + boneY * length
			eb.align_roll(boneZ)
			end_editmode()
		else:
			start_posemode(arm_obj)
			arm_obj.pose.bones[bone_name].matrix = new_bone_mat
			end_posemode()
	else:
		raise TypeError("set_object_transform: error with argument 1")

# Makes an object's copy		
def copy_object(extobj):
	if type(extobj) == bpy.types.Object:
		newobj = extobj.copy()
		bpy.context.scene.objects.link(newobj)
		return newobj
	else:
		raise TypeError("copy_object: error with argument 1")

# Deletes a specified extended object
def delete_object(extobj):
	if type(extobj) == bpy.types.Object:
		if bpy.context.scene.objects.get(extobj.name) == extobj:
			bpy.context.scene.objects.unlink(extobj)
		if extobj.users == 0:
			bpy.data.objects.remove(extobj)
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		arm = obj.data
		start_editmode(obj)
		arm.edit_bones.remove(arm.edit_bones[bone_name])
		end_editmode()
	else:
		raise TypeError("delete_object: error with argument 1")

# Replace the first object's data with the second object's data
# Use it only for objects which contain meshes
def replace_object(extobj, srcextobj):
	if type(extobj) == bpy.types.Object and type(srcextobj) == bpy.types.Object and extobj.type == srcextobj.type:
		# Copy material slots
		if len(extobj.material_slots) != len(srcextobj.material_slots):
			bpy.context.scene.objects.active = extobj
			delta = len(srcextobj.material_slots) - len(extobj.material_slots)
			if delta > 0:
				for j in range(0, delta):
					bpy.ops.object.material_slot_add()
			else:
				for j in range(0, -delta):
					bpy.ops.object.material_slot_remove()
		
		# Setup material slots
		for j in range(0, len(extobj.material_slots)):
			extobj.material_slots[j].link = "OBJECT"
			extobj.material_slots[j].material = srcextobj.material_slots[j].material
		
		# Link to another data
		extobj.data = srcextobj.data
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		pass # The following code is not finished
		#obj = extobj[0]
		#bone_name = extobj[1]
		#pose = obj.pose
		#pose.bones[bone_name].custom_shape = srcextobj
	else:
		raise TypeError("replace_object: error with argument 1")

# Returns an object with a specified name.
# The function returns None if not found.
def find_object_by_name(name):
	(obj_name, bone_name) = split_bone_name(name)
	o = bpy.data.objects.get(obj_name)
	if o != None:
		if bone_name == "":
			return o
		if o.type == "ARMATURE":
			arm = o.data
			for b in arm.bones:
				if b.name == bone_name:
					return ((o, b.name))
	return None

# Finds unique name for object based a specified name.
def unique_object_name(name):
	txt = name.rstrip("0123456789")
	idx = 0
	while(True):
		idx = idx + 1
		sidx = str(idx)
		if(len(sidx) < 2):
			sidx = "0" + sidx
		name = txt + sidx
		if find_object_by_name(name) == None:
			break
	return name

# Returns number of vertices in object if this is a mesh; returns 0 if this is not a mesh.
def calculate_num_verts(extobj):
	if type(extobj) == bpy.types.Object and extobj.type == "MESH":
		return len(extobj.data.vertices)
	return 0

# Returns number of faces in object if this is a mesh; returns 0 if this is not a mesh.
def calculate_num_faces(extobj):
	if type(extobj) == bpy.types.Object and extobj.type == "MESH":
		return len(extobj.data.polygons)
	return 0
	
# Returns number of materials used by an object.
def calculate_num_materials(extobj):
	if type(extobj) == bpy.types.Object:
		return len(extobj.material_slots)
	return 0

# Checks if an object is visible (in Editor).	
def is_visible(extobj):
	if type(extobj) == bpy.types.Object:
		return not(extobj.hide)
	return False

# Sets if an object is visible (in Editor).	
def show(extobj, show):
	if type(extobj) == bpy.types.Object:
		extobj.hide = not(show)

# Checks if an object is visible (for Renderer).
def is_renderable(extobj):
	if type(extobj) == bpy.types.Object:
		return not(extobj.hide_render)
	return False

# Sets if an object is visible (for Renderer).
def set_renderable(extobj, renderable):
	if type(extobj) == bpy.types.Object:
		extobj.hide_render = not(renderable)

# Checks if an object is drawn as bounding box.
def get_box_mode(extobj):
	if type(extobj) == bpy.types.Object:
		return extobj.draw_type == "BOUNDS"
	return False

# Sets if an object is drawn as bounding box.
def set_box_mode(extobj, boxmode):
	if type(extobj) == bpy.types.Object:
		if(boxmode):
			extobj.draw_type = "BOUNDS"
		else:
			extobj.draw_type = "TEXTURED"

# Checks if an object is transparent.
def is_transparent(extobj):
	if type(extobj) == bpy.types.Object:
		return extobj.transparent
	return False

# Sets if an object is transparent.
def set_transparent(extobj, trasp):
	#if type(extobj) == bpy.types.Object:
	#	extobj.show_transparent = transp
	#	if transp:
	#		for slot in extobj.material_slots:
	#			slot.material.alpha = 0.250
	pass

# Returns object's color.
def get_wire_color(extobj):
	if type(extobj) == bpy.types.Object:
		return extobj.color[0:3]
	return [0, 0, 0]

# Changes object's color
def set_wire_color(extobj, clr):
	if type(extobj) == bpy.types.Object:
		extobj.color = [clr[0], clr[1], clr[2], extobj.color[3]]
	
# Checks if an object is selected.
def is_selected(extobj):
	if type(extobj) == bpy.types.Object:
		if extobj.type == "ARMATURE":
			return False
		return extobj.select
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		arm = obj.data
		return (obj.select and arm.bones[bone_name].select)
	else:
		raise TypeError("select: error with argument 1")

# Selects an object.
def select(extobj):
	if type(extobj) == bpy.types.Object:
		extobj.select = True
		bpy.context.scene.objects.active = extobj
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		arm = obj.data
		obj.select = True
		bone = arm.bones[bone_name]
		bone.select = True
		arm.bones.active = bone
	else:
		raise TypeError("select: error with argument 1")
		
# Deselects an object.
def deselect(extobj):
	if type(extobj) == bpy.types.Object:
		extobj.select = False
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		arm = obj.data
		arm.bones[bone_name].select = False
	else:
		raise TypeError("select: error with argument 1")		

# Deselect all the objects.
def deselect_all():
	bpy.ops.object.select_all(action = "DESELECT")
	for arm in bpy.data.armatures:
		for b in arm.bones:
			b.select = False
	
# Starts object's editing.
def start_editmode(extobj):
	if type(extobj) == bpy.types.Object:
		if bpy.context.edit_object != extobj:
			if bpy.context.edit_object != None:
				bpy.ops.object.editmode_toggle()
			bpy.context.scene.objects.active = extobj
			bpy.ops.object.editmode_toggle()
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		bone_name = extobj[1]
		start_editmode(obj)
		arm = obj.data
		arm.bones.active = arm.bones[bone_name]
	else:
		raise TypeError("start_editmode: error with argument 1")
		
# Ends object's editing.
def end_editmode():
	if bpy.context.edit_object != None:
		bpy.ops.object.editmode_toggle()
		
# Starts object's editing.
def start_posemode(extobj):
	if type(extobj) == bpy.types.Object and extobj.type == "ARMATURE":
		if not(bpy.context.active_pose_bone in list(extobj.pose.bones)):
			if bpy.context.active_pose_bone != None:
				bpy.ops.object.posemode_toggle()
			bpy.context.scene.objects.active = extobj
			bpy.ops.object.posemode_toggle()
	elif type(extobj) == tuple and len(extobj) == 2 and type(extobj[0]) == bpy.types.Object and type(extobj[1]) == str:
		obj = extobj[0]
		start_posemode(obj)
	else:
		raise TypeError("start_editmode: error with argument 1")
		
# Ends object's editing.
def end_posemode():
	if bpy.context.active_pose_bone != None:
		bpy.ops.object.posemode_toggle()
		