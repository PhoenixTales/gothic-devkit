# math_utils.py: provides utilities to operate vectors, 
# matrices, quaternions, and colors.
#--------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#--------------------------------------------------------------------------------------------------


# Import the required modules
from mathutils import *
from math import *


#----------------------------------
# Color operations               #
#----------------------------------

def black_color():
	return [0, 0, 0]
	
def new_color(r, g, b):
	return [r, g, b]

def set_red(clr, r):
	clr[0] = r

def set_green(clr, g):
	clr[1] = g

def set_blue(clr, b):
	clr[2] = b
	
def get_red(clr):
	return clr[0]

def get_green(clr):
	return clr[1]

def get_blue(clr):
	return clr[2]


	

#----------------------------------
# Vector operations               #
#----------------------------------

def zero_vector():
	return Vector([0, 0, 0])
	
def new_vector(x, y, z):
	return Vector([x, y, z])

def set_x(vec, a):
	vec.x = a

def set_y(vec, a):
	vec.y = a
	
def set_z(vec, a):
	vec.z = a

def get_x(vec):
	return vec.x

def get_y(vec):
	return vec.y

def get_z(vec):
	return vec.z
	
def get_vector_length(vec):
	return vec.length

def dot_product(vec1, vec2):
	return vec1.dot(vec2)
	
def lerp_vector(vec1, vec2, t):
	return vec1.lerp(vec2, t)

	

#----------------------------------
# Quaternion operations           #
#----------------------------------

# My own implementation of quaternions

# Provides a quaternion representation for orientation in 3D space using an angle in radians and a rotation axis.
# Rotations follow the left-hand-rule (like the "AngAxis" class in 3dsmax sdk).
def new_quaternion(axis, angle): # angle 
	sin_a = sin(angle / 2)
	cos_a = cos(angle / 2)
	axis_norm = axis.normalized()
	qx = axis_norm.x * sin_a
	qy = axis_norm.y * sin_a
	qz = axis_norm.z * sin_a
	qw = cos_a
	return (qx, qy, qz, qw)

# Returns the identity quaternion (which means "no rotation"): w = 1, x = y = z = 0
def identity_quaternion():
	qx = 0
	qy = 0
	qz = 0
	qw = 1
	return (qx, qy, qz, qw)

def get_angle(q):
	qw = q[3]
	cos_a = qw;
	angle = acos( cos_a ) * 2
	return angle

def get_axis(q):
	qx = q[0]
	qy = q[1]
	qz = q[2]
	qw = q[3]
	
	cos_a = qw;
	sin_a = sqrt( 1 - cos_a * cos_a )
	
	# This check prevents from division by zero
	if abs( sin_a ) < 0.000001: 
		return Vector((0, 0, 1))
	
	axis = Vector(((qx / sin_a), (qy / sin_a), (qz / sin_a)))
	return axis

def slerp_quaternion(q1, q2, t):
	qres = q1.slerp(q2, t)
	return qres

	
#----------------------------------
# Matrix operations               #
#----------------------------------

# My own implementation of the matrix operation like the "Matrix3" class in the 3dsmax sdk.

# 4x4 3D transformation matrix; however only 12 components are used.
# Any matrix follows the template:
# m00 m01 m02 0
# m10 m11 m12 0
# m20 m21 m22 0
# m30 m31 m32 1
# The first 9 componenets represents rotation and scale transformations,
# and the rest three components (m30, m31, m32) represents translation.

def zero_matrix():
	return Matrix(([0,0,0,0], [0,0,0,0], [0,0,0,0], [0,0,0,0]))

def identity_matrix():
	return Matrix(([1,0,0,0], [0,1,0,0], [0,0,1,0], [0,0,0,1]))

def new_matrix(row0, row1, row2, row3):
	# Matrix is constructed by rows in Blender
	return Matrix(([row0.x, row0.y, row0.z, 0], [row1.x, row1.y, row1.z, 0], [row2.x, row2.y, row2.z, 0], [row3.x, row3.y, row3.z, 1]))

def is_identity_matrix(mat):
	return mat == identity_matrix()

# Returns the zeroth row as three-components vector.
# The zeroth row represents the unit vector along the first axis of a local coordinate system 
# projected on the three axes of the world coordinate system.
def get_row0(mat):
	return Vector(mat[0][0:3])

# Returns the first row as three-components vector.
# The first row represents the unit vector along the second axis of a local coordinate system 
# projected on the three axes of the world coordinate system.
def get_row1(mat):
	return Vector(mat[1][0:3])

# Returns the second row as three-components vector.
# The second row represents the unit vector along the third axis of a local coordinate system 
# projected on the three axes of the world coordinate system.
def get_row2(mat):
	return Vector(mat[2][0:3])

# Returns the third row as three-components vector.
# The third row represents the origin of a local coordinate system
# in the world coordinate system.
def get_row3(mat):
	return Vector(mat[3][0:3])

def get_row(mat, rowIndex):
	return Vector(mat[rowIndex][0:3])

def set_row0(mat, row0):
	mat[0][0:3] = row0

def set_row1(mat, row1):
	mat[1][0:3] = row1

def set_row2(mat, row2):
	mat[2][0:3] = row2

def set_row3(mat, row3):
	mat[3][0:3] = row3

def set_row(mat, rowIndex, row):
	mat[rowIndex][0:3] = row
	
# Inverse matrix
def inverse_matrix(mat):
	return mat.inverted()

# Determinant
def determinant(mat):
	return mat.determinant()

# Multiply vector by matrix, a vector assumed to be row vector.
# i.e. a row vector transformed by a matrix.
def multiply_vector_matrix(vec, mat):
	res = Vector((0,0,0))
	for i in range(0, 3):
		res[i] = vec[0] * mat[0][i] + vec[1] * mat[1][i] + vec[2] * mat[2][i] + mat[3][i]
	return res

# Multiply matrix by matrix (linear algebra rules).
def multiply_matrix_matrix(lmat, rmat):
	res = Matrix().to_4x4()
	for i in range(0, 4):
		for j in range(0, 4):
			res[i][j] = lmat[i][0] * rmat[0][j] + lmat[i][1] * rmat[1][j] + lmat[i][2] * rmat[2][j] + lmat[i][3] * rmat[3][j]
	return res
	
# Create transformation matrix for translation by translation vector
def translation_matrix(pt):
	return Matrix(([1, 0, 0, 0], [0, 1, 0, 0], [0, 0, 1, 0], [pt.x, pt.y, pt.z, 1]))

# Get vector which is a translation part of a specified transformation matrix
def get_translation_part(mat):
	return get_row3(mat)

# Create transformation matrix for rotation by quaternion
def rotation_matrix(q):
	x = q[0]
	y = q[1]
	z = q[2]
	w = q[3]
	
	xx = x * x
	xy = x * y
	xz = x * z
	xw = x * w
	
	yy = y * y
	yz = y * z
	yw = y * w
	
	zz = z * z
	zw = z * w
	
	mat = Matrix().to_4x4()
	mat[0][0] = 1 - 2 * ( yy + zz )
	mat[0][1] =     2 * ( xy - zw )
	mat[0][2] =     2 * ( xz + yw )
	
	mat[1][0] =     2 * ( xy + zw )
	mat[1][1] = 1 - 2 * ( xx + zz )
	mat[1][2] =     2 * ( yz - xw )
	
	mat[2][0] =     2 * ( xz - yw )
	mat[2][1] =     2 * ( yz + xw )
	mat[2][2] = 1 - 2 * ( xx + yy )
	
	mat[0][3] = 0
	mat[1][3] = 0
	mat[2][3] = 0
	mat[3][0] = 0
	mat[3][1] = 0
	mat[3][2] = 0
	mat[3][3] = 1
	return mat

# Get quaternion which is a rotation part of a specified transformation matrix
def get_rotation_part(mat):
	# extract translation - not needed
	#translation = Vector((0, 0, 0))
	#translation[0] = mat[3][0]
	#translation[1] = mat[3][1]
	#translation[2] = mat[3][2]
	
	# extract scale - not returned, but used in calculations
	scale = Vector((1, 1, 1))
	scale[0] = Vector((mat[0][0], mat[0][1], mat[0][2])).length
	scale[1] = Vector((mat[1][0], mat[1][1], mat[1][2])).length
	scale[2] = Vector((mat[2][0], mat[2][1], mat[2][2])).length
	
	# extract rotation
	if 1 + mat[0][0] + mat[1][1] + mat[2][2] > 0.000001:
		qw = sqrt(1 + mat[0][0] / scale[0] + mat[1][1] / scale[1] + mat[2][2] / scale[2]) / 2
		qx = (mat[2][1] / scale[2] - mat[1][2] / scale[1]) / (4 * qw)
		qy = (mat[0][2] / scale[0] - mat[2][0] / scale[2]) / (4 * qw)
		qz = (mat[1][0] / scale[1] - mat[0][1] / scale[0]) / (4 * qw)
	elif mat[0][0] > mat[1][1] and mat[0][0] > mat[2][2]:
		qx = sqrt(1 + mat[0][0] / scale[0] - mat[1][1] / scale[1] - mat[2][2] / scale[2]) / 2
		qy = (mat[0][1] / scale[0] + mat[1][0] / scale[1]) / (4 * qx)
		qz = (mat[0][2] / scale[0] + mat[2][0] / scale[2]) / (4 * qx)
		qw = (mat[2][1] / scale[2] - mat[1][2] / scale[1]) / (4 * qx)
	elif mat[1][1] > mat[2][2]:
		qy = sqrt(1 + mat[1][1] / scale[1] - mat[0][0] / scale[0] - mat[2][2] / scale[2]) / 2
		qx = (mat[0][1] / scale[0] + mat[1][0] / scale[1]) / (4 * qy)
		qz = (mat[1][2] / scale[1] + mat[2][1] / scale[2]) / (4 * qy)
		qw = (mat[0][2] / scale[0] - mat[2][0] / scale[2]) / (4 * qy)
	else:
		qz = sqrt(1 + mat[2][2] / scale[2] - mat[0][0] / scale[0] - mat[1][1] / scale[1]) / 2
		qx = (mat[0][2] / scale[0] + mat[2][0] / scale[2]) / (4 * qz)
		qy = (mat[1][2] / scale[1] + mat[2][1] / scale[2]) / (4 * qz)
		qw = (mat[1][0] / scale[1] - mat[0][1] / scale[0]) / (4 * qz)
	
	rotation = (qx, qy, qz, qw)
	return rotation
	
# Prepare matrix for using by KrxImpExp's scripts after getting it from Blender.
def prepare_matrix(mat):
	matR = mat.transposed()
	return matR
	
# Prepare matrix for using by Blender after calculating it in KrxImpExp' scripts.
def unprepare_matrix(mat):
	matR = mat.transposed()
	return matR	
	