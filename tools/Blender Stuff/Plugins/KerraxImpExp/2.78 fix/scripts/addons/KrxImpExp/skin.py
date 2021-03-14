# skin.py: Utilities to work with skin based on the armature modifier.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------


# Import required modules
import bpy
from KrxImpExp.object import *


# Returns the list of the supported skin types
def get_supported_skin_types():
	return ["Armature modifier"]

# Returns True if the object has the armature modifier applied.
def is_skin_object(obj):
	if type(obj) == bpy.types.Object:
		return obj.find_armature() != None
	else:
		return False

# Special class to setup the armature modifier.
class SkinData:
	def __init__(self, skinobj, skintype = "Armature modifier"):
		if type(skinobj) != bpy.types.Object:
			raise TypeError("SkinData.__init__: error with argument 2")
		self.__skintype = skintype
		self.__skinobj = skinobj
		self.__mesh = skinobj.data
		self.__armobj = skinobj.find_armature()
	
	def get_skin_type(self):
		return self.__skintype
	
	def add_bones(self, extbones):
		for extbone in extbones:
			if type(extbone) != tuple or len(extbone) != 2 or type(extbone[0]) != bpy.types.Object or type(extbone[1]) != str:
				raise TypeError("SkinData.add_bones: error with argument 2")
			
			if self.__armobj == None:
				self.__armobj = extbone[0]
				newmod = self.__skinobj.modifiers.new("Armature", "ARMATURE")
				newmod.object = self.__armobj
				newmod.use_vertex_groups = True
				set_parent(self.__skinobj, self.__armobj)
				self.__armobj.data.show_axes = True
			
			bone_name = extbone[1]
		
		for extbone in extbones:
			bone_name = extbone[1]
			self.__skinobj.vertex_groups.new(bone_name)
	
	def get_num_bones(self):
		return len(self.__obj.vertex_groups)
	
	def get_bone(self, index):
		bone_name = self.__skinobj.vertex_groups[index].name
		return ((self.__armobj, bone_name))
	
	def get_num_verts(self):
		return len(self.__mesh.vertices)
	
	def set_vert_weights(self, vertIdx, extbones, weights):
		for i in range(0, len(extbones)):
			extbone = extbones[i]
			weight = weights[i]
			bone_name = extbone[1]
			vg = self.__skinobj.vertex_groups[bone_name]
			vg.add([vertIdx], weight, "REPLACE")
	
	def get_vert_num_weights(self, vertIdx):
		return len(self.__mesh.vertices[vertIdx].groups)
	
	def get_vert_weight(self, vertIdx, vertBoneIdx):
		return self.__mesh.vertices[vertIdx].groups[vertBoneIdx].weight
	
	def get_vert_weight_bone(self, vertIdx, vertBoneIdx):
		groupIndex = self.__mesh.vertices[vertIdx].groups[vertBoneIdx].group
		bone_name = self.__skinobj.vertex_groups[groupIndex].name
		return ((self.__armobj, bone_name))
