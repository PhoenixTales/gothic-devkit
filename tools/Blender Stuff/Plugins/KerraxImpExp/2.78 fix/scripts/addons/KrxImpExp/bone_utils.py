# bone_utils.py: Utilities to work with bones.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------

from KrxImpExp.math_utils import *

# Split a bone's full name into two parts: prefix (before "BIP") and name itself.
def split_bone_name(name):
	bippos = max(name.find("Bip"), name.find("BIP"), name.find("bip"))
	if bippos == -1:
		prefix = name
		bone_name = ""
	elif bippos == 0:
		prefix = "Armature"
		bone_name = name
	else:
		prefix = name[:bippos]
		bone_name = name[bippos:]
	return ((prefix, bone_name))

# Make a bone full name from its parts: prefix (before "BIP") and name itself.
def make_bone_name(prefix, bone_name):
	if prefix == "Armature":
		name = bone_name
	else:
		name = prefix + bone_name
	return name

# Matrix to rotate local axes of all bones for better looking of models in Blender which were created in 3ds max.
# (In 3dsmax any bone lies along its local X direction, however in Blender any bone lies along its local Y direction,
# so we can want to rotate axes around Z direction to match the directions).
__bone_prep_rotation_matrix = Quaternion(Vector((0,0,1)), pi/2).to_matrix()
#__bone_prep_rotation_matrix = Matrix.Identity(3) #test
__bone_prep_rotation_matrix_inverted = __bone_prep_rotation_matrix.inverted()
	
# Prepare bone matrix for using by KrxImpExp's scripts after getting it from Blender.
def prepare_bone_matrix(mat):
	global __bone_prep_rotation_matrix
	matR = prepare_matrix(mat)
	x = matR[3][0]
	y = matR[3][1]
	z = matR[3][2]
	matR = matR.to_3x3()
	matR.transpose()
	matR = matR * __bone_prep_rotation_matrix
	matR.transpose()
	matR = matR.to_4x4()
	matR[3][0] = x
	matR[3][1] = y
	matR[3][2] = z
	return matR
		
# Prepare bone matrix for using by Blender after calculating it in KrxImpExp' scripts.
def unprepare_bone_matrix(mat):
	global __bone_prep_rotation_matrix_inverted
	matR = mat.copy()
	x = matR[3][0]
	y = matR[3][1]
	z = matR[3][2]
	matR = matR.to_3x3()
	matR.transpose()
	matR = matR * __bone_prep_rotation_matrix_inverted
	matR.transpose()
	matR = matR.to_4x4()
	matR[3][0] = x
	matR[3][1] = y
	matR[3][2] = z
	matR = unprepare_matrix(matR)
	return matR
	
	