# material.py: material utilities.
#------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#------------------------------------------------------


# Import required modules.
import bpy
from KrxImpExp.image_search import *

# Creates a new material, unless one exists already
def new_material(name):
	i = bpy.data.materials.find(name.upper())
	if i == -1:
		mat = bpy.data.materials.new(name.upper())
		return mat
	else:
		mat = bpy.data.materials[i]
		return mat

# Returns the name of the material.
def get_material_name(mat):
	# material's custom properties "use_long_name", "long_name" were designed for using 
	# in older version of Blender which supported names of materials up to 21 characters. 
	# Blender 2.62(?) and later support materials' names up to 63 characters, 
	# so these custom properties are not required now.
	if type(mat.get("long_name")) == str: 
		if mat.get("use_long_name") == 1:
			newmatname = mat.get("long_name")
			oldmatname = mat.name
			mat.name = newmatname
			if mat.name != newmatname:
				mat.name = oldmatname
				return newmatname
		del mat["long_name"]
		del mat["use_long_name"]
	return mat.name.upper()

# Changes the name of the material
def set_material_name(mat, name):
	mat.name = name.upper()

# Returns a string with filename (without path) of a diffuse map if it's used by the material.
# Returns an empty string if there is no diffuse map.
def get_diffuse_map_filename(mat):
	im = find_image_in_material_slots(mat)
	if (im == None):
		return ""
	filepath = im.filepath
	filename = filepath
	i = max(filename.rfind(os.sep), filename.rfind(os.altsep))
	if (i != -1):
		filename = filename[i+1:]
	return filename

# Sets a diffuse map to be used by the material.
def set_diffuse_map_filename(mat, filename):
	image = load_image(filename)
	
	i = bpy.data.textures.find(filename)
	if i == -1:
		texture = bpy.data.textures.new(filename, "IMAGE")
		texture.image = image
	else:
		texture = bpy.data.textures[i]
	
	slots = mat.texture_slots
	for i in range(0, len(slots)):
		slots.clear(i)
	slot = mat.texture_slots.create(0)
	slot.texture = texture
	slot.texture_coords = "UV"
	mat.use_textures[0] = True

# Returns diffuse color of the material.
def get_diffuse_color(mat):
	return mat.diffuse_color

# Sets diffuse color of the material.
def set_diffuse_color(mat, clr):
	mat.diffuse_color = clr
	
# Finds a first image in the material's slot.
# The function returns None if this material does not contain a texture with image.
def find_image_in_material_slots(mat):
	slots = mat.texture_slots
	tex = None
	for j in range(len(slots) - 1, -1, -1):
		slot = slots[j]
		if (slot != None and slot.texture != None and mat.use_textures[j]):
			tex = slot.texture
			break
	if (tex == None):
		return None
	if (tex.type != "IMAGE"):
		return None
	im = tex.image
	return im

# Finds a material which uses the specified image; if not found, the function creates a new material
# So this function never fails.
def get_material_by_image(image):
	for mat in bpy.data.materials:
		for slot in mat.texture_slots:
			if slot != None:
				tex = slot.texture
				if tex.type == "IMAGE":
					if tex.image == image:
						return mat
	
	texture = None
	for tex in bpy.data.textures:
		if tex.type == "IMAGE":
			if tex.image == image:
				texture = tex
				break
	
	if texture == None:
		texture = bpy.data.textures.new(image.name, "IMAGE")
		texture.image = image
	
	mat = bpy.data.materials.new(image.name)
	mat.name = image.name
	slot = mat.texture_slots.create(0)
	slot.texture = texture
	slot.texture_coords = "UV"
	return mat	
