# mesh.py: mesh utilities.
#--------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#--------------------------------------------------------------------------------------------------


import bpy
from KrxImpExp.material import *
from KrxImpExp.math_utils import *
from KrxImpExp.gui import *

# Creates a new object linked to mesh
def new_mesh_object(name):
	msh = bpy.data.meshes.new(name)
	obj = bpy.data.objects.new(name, object_data = msh)
	bpy.context.scene.objects.link(obj)
	return obj

# Checks if this object is linked to a mesh
def is_mesh_object(obj):
	return (type(obj) == bpy.types.Object and obj.type == "MESH")

# Mesh class which is styled in 3dsmax
class MeshData:
	# Constructor
	def __init__(self, obj):
		if type(obj) == bpy.types.Object and obj.type == "MESH":
			self.__obj = obj
			if len(obj.modifiers) == 0 and obj.data.shape_keys == None:
				self.__mesh = obj.data
				self.__remove_mesh = False
			else:
				self.__mesh = obj.to_mesh(bpy.context.scene, True, "RENDER")
				self.__remove_mesh = True
			self.__init_geometry()
			self.__init_mapping()
		else:
			raise TypeError("MeshData.__init__: error with argument 2")
	
	# Internal function: Getting of geometry from the Blender's mesh
	def __init_geometry(self):
		obj  = self.__obj
		mesh = self.__mesh
		self.__verts = [v.co for v in mesh.vertices]
		self.__faces = []
		self.__facemats = []
		
		for p in mesh.polygons:
			mat = None
			matIndex = p.material_index
			if 0 <= matIndex and matIndex < len(obj.material_slots):
				mat = obj.material_slots[matIndex].material
			if mat == None and mesh.uv_textures.active != None:
				tf = mesh.uv_textures.active.data[p.index]
				if tf.image != None:
					mat = get_material_by_image(tf.image)
			
			vs = p.vertices
			for i in range(2, p.loop_total):
				va = mesh.loops[p.loop_start].vertex_index
				vb = mesh.loops[p.loop_start + i - 1].vertex_index
				vc = mesh.loops[p.loop_start + i].vertex_index
				self.__faces.append([va, vb, vc])
				self.__facemats.append(mat)
	
	# Internal function: Getting of texture coordinates from the Blender's mesh
	def __init_mapping(self):
		obj = self.__obj
		mesh = self.__mesh
		self.__tverts = []
		self.__tvfaces = []
		
		if mesh.uv_layers.active != None:
			uv_layer = mesh.uv_layers.active.data
			for uvl in uv_layer:
				self.__tverts.append(uvl.uv)
			for p in mesh.polygons:
				for i in range(2, p.loop_total):
					ta = p.loop_start
					tb = p.loop_start + i - 1
					tc = p.loop_start + i
					self.__tvfaces.append([ta, tb, tc])
	
	# Destructor
	def __deinit__(self):
		if self.__remove_mesh:
			bpy.data.meshes.remove(self.__mesh)
	
	# Public: Getting properties of mesh
	def get_num_verts(self):
		return len(self.__verts)
	
	def get_vert(self, vertIndex):
		return self.__verts[vertIndex]
	
	def get_num_faces(self):
		return len(self.__faces)
	
	def get_face(self, faceIndex):
		return self.__faces[faceIndex]
	
	def get_face_material(self, faceIndex):
		return self.__facemats[faceIndex]
	
	def get_num_tverts(self):
		return len(self.__tverts)
	
	def get_tvert(self, tVertIndex):
		return self.__tverts[tVertIndex]
	
	def get_num_tvfaces(self):
		return self.get_num_faces()
	
	def get_tvface(self, faceIndex):
		return self.__tvfaces[faceIndex]
	
	# Public: Setting properties of mesh
	def set_num_verts(self, newNumVerts):
		self.__verts = [Vector((0, 0, 0)) for i in range(0,  newNumVerts)]
	
	def set_vert(self, vertIndex, pt):
		self.__verts[vertIndex] = pt
	
	def set_num_faces(self, newNumFaces):
		self.__faces = [[0, 0, 0] for i in range(0,  newNumFaces)]
		self.__facemats  = [None for i in range(0,  newNumFaces)]
	
	def set_face(self, faceIndex, fc):
		self.__faces[faceIndex] = fc
	
	def set_face_material(self, faceIndex, mat):
		self.__facemats[faceIndex] = mat
	
	def set_num_tverts(self, newNumTVerts):
		self.__tverts = [Vector((0, 0)) for i in range(0, newNumTVerts)]
	
	def set_tvert(self, tVertIndex, uvVert):
		self.__tverts[tVertIndex] = uvVert
	
	def set_num_tvfaces(self, newNumTVFaces):
		self.__tvfaces = [[0, 0, 0] for i in range(0,  newNumTVFaces)]
	
	def set_tvface(self, faceIndex, tvFace):
		self.__tvfaces[faceIndex] = tvFace

	# Update mesh after it has been changed
	def update(self):
		obj = self.__obj
		mesh = self.__mesh
		
		# Initialise the mesh's vertices and faces
		if len(mesh.vertices) != 0 or len(mesh.polygons) != 0: # the "Mesh.from_pydata" function works only on clean meshes
			return
		print("Mesh ", mesh.name, ": generating from pydata ", len(self.__verts), " verts, ", len(self.__faces), " faces...")
		mesh.from_pydata(self.__verts, [], self.__faces)
		print("Mesh ", mesh.name, ": generating from pydata done!")

		# Remove duplications from list of all the materials
		print("Mesh ", mesh.name, ": creating material slots...")
		matSet = list(set(self.__facemats))

		# Align the size of obj.material_slots to the size of matSet
		if len(obj.material_slots) != len(matSet):
			bpy.context.scene.objects.active = obj
			delta = len(matSet) - len(obj.material_slots)
			if delta > 0:
				for j in range(0, delta):
					bpy.ops.object.material_slot_add()
			else:
				for j in range(0, -delta):
					bpy.ops.object.material_slot_remove()
		
		# Setup material slots
		matDict = {}
		for j in range(0, len(matSet)):
			obj.material_slots[j].link = "OBJECT"
			obj.material_slots[j].material = matSet[j]
			matDict[matSet[j]] = j
		print("Mesh ", mesh.name, ": creating material slots done!")

		# Setup material indices for each face
		print("Mesh ", mesh.name, ": applying ", len(matSet), " materials to ", len(mesh.polygons), " faces...")
		mesh.update_tag()
		for p in mesh.polygons:
			faceIndex = p.index
			if (faceIndex % 1000) == 999:
				show_progress_bar("Applying materials to faces", faceIndex / len(mesh.polygons) * 100)
			mat = self.__facemats[faceIndex]
			matIndex = matDict[mat]
			p.material_index = matIndex # works slowly (is there a way to optimize this line?)
		print("Mesh ", mesh.name, ": applying materials to faces done!")
		
		# Setup texture coordinates
		print("Mesh ", mesh.name, ": applying uv coordinates to ", len(mesh.polygons), " faces...")
		if len(self.__tvfaces) != 0:
			if mesh.uv_layers.active == None:
				mesh.uv_textures.new()
			if mesh.uv_layers.active != None:
				uv_layer = mesh.uv_layers.active.data
				tfs = mesh.uv_textures.active.data
				for p in mesh.polygons:
					faceIndex = p.index
					if (faceIndex % 1000) == 999:
						show_progress_bar("Applying uv coordinates to faces", faceIndex / len(mesh.polygons) * 100)
					for li in p.loop_indices:
						uv = self.__tverts[self.__tvfaces[faceIndex][li - p.loop_start]]
						uv_layer[li].uv = uv # works slowly (is there a way to optimize this line?)
					tf = tfs[faceIndex]
					matIndex = p.material_index
					if matIndex < len(obj.material_slots):
						mat = obj.material_slots[matIndex].material
						if mat != None:
							tf.image = find_image_in_material_slots(mat)
		print("Mesh ", mesh.name, ": applying uv coordinates done!")
		
		# Calculate edges
		print("Mesh ", mesh.name, ": updating...")
		mesh.update(calc_edges = True)
		print("Mesh ", mesh.name, ": updating done!")
		
		mesh.calc_normals()
		mesh.show_double_sided = False

		# We must NOT call "validate" here because it will try to remove one side of each portal!
		#print("Mesh ", mesh.name, ": validating...")
		#mesh.validate(verbose = False)
		#print("Mesh ", mesh.name, ": validating done!")
		
		# Return the list of all the objects which were changed
		return [obj]

# Face
def new_face(v0, v1, v2):
	return [v0, v1, v2]
	
def get_face_vert(face, vertIndexInFace):
	return face[vertIndexInFace]

def set_face_vert(face, vertIndexInFace, vertIndex):
	face[vertIndexInFace] = vertIndex

# Face texture
def new_tvface(t0, t1, t2):
	return [t0, t1, t2]
	
def get_tvface_vert(tvface, vertIndexInFace):
	return tvface[vertIndexInFace]

def set_tvface_vert(tvface, vertIndexInFace, tVertIndex):
	tvface[vertIndexInFace] = tVertIndex

# Mapping coordinates (uv)
def new_uvvert(u, v):
	return Vector((u, v))
	
def zero_uvvert():
	return Vector((0, 0))

def set_u(uvvert, u):
	uvvert[0] = u

def set_v(uvvert, v):
	uvvert[1] = v

def get_u(uvvert):
	return uvvert[0]

def get_v(uvvert):
	return uvvert[1]
		
# Welds vertices
def weld_vertices(obj, threshold):
	# Welding seems to work incorrectly
	#bpy.ops.object.select_name(name = obj.name)
	#objmode = obj.mode
	#if objmode != "EDIT":
	#	bpy.ops.object.editmode_toggle()
	#bpy.ops.mesh.remove_doubles(threshold)
	#if objmode != "EDIT":
	#	bpy.ops.object.editmode_toggle()
	return len(obj.data.vertices)
