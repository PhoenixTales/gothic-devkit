# impexp.py: Helper utilities to make importers and exporters.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------


# Import required modules
import bpy
from bpy.props import *
from bpy_extras.io_utils import ImportHelper, ExportHelper

from KrxImpExp.image_search import *

from KrxImpExp.animation import *
from KrxImpExp.bone import *
from KrxImpExp.bone_utils import *
from KrxImpExp.file import *
from KrxImpExp.gui import *
from KrxImpExp.material import *
from KrxImpExp.math_utils import *
from KrxImpExp.mesh import *
from KrxImpExp.numconv import *
from KrxImpExp.object import *
from KrxImpExp.scene import *
from KrxImpExp.skin import *
from KrxImpExp.string import *

# Gets the objects' selection
def store_selection():
	return [obj.name for obj in bpy.data.objects if obj.select]

# Sets the objects' selection
def restore_selection(selnames):
	for obj in bpy.data.objects:
		obj.select = (obj.name in selnames)

# Global dictionaries of registed importers and registered exporters
__registered_importers = {}
__registered_exporters = {}
		
# Registers an importer; impname is a function's name which gets single parameter, filename.
def register_importer(impname, fileext, description):
	class __PrivateOperator(bpy.types.Operator, ImportHelper):
		bl_idname = "import_scene." + impname.lower()
		bl_label = "Import " + fileext.upper()
		
		filename_ext = "." + fileext
		quiet = BoolProperty(default=False, options={"HIDDEN"})
		filter_glob = StringProperty(default="*." + fileext, options={'HIDDEN'})
		
		def execute(self, context):
			if file_exists(self.filepath):
				end_posemode()
				end_editmode()
				sel = store_selection()
				s = "from KrxImpExp" + " import " + impname
				exec(s)
				s = impname + "." + impname + "('" + self.filepath.replace("\\", "\\\\") + "', " + str(self.quiet) + ")"
				exec(s)
				restore_selection(sel)
			return {'FINISHED'}
			
	bpy.utils.register_class(__PrivateOperator)
	
	def __menu_func_import(self, context):
		self.layout.operator(__PrivateOperator.bl_idname, text = description + " (." + fileext + ")")
	
	bpy.types.INFO_MT_file_import.append(__menu_func_import)
	
	global __registered_importers
	__registered_importers[impname] = [__menu_func_import, __PrivateOperator]

# Unregisters an importer
def unregister_importer(impname):
	global __registered_importers
	bpy.types.INFO_MT_file_import.remove(__registered_importers[impname][0])
	bpy.utils.unregister_class(__registered_importers[impname][1])
	del __registered_importers[impname]

# Registers an exporter; expname is a function's name which gets single parameter, filename.
def register_exporter(expname, fileext, description):
	class __PrivateOperator(bpy.types.Operator, ExportHelper):
		bl_idname = "export_scene." + expname.lower()
		bl_label = "Export " + fileext.upper()
		
		filename_ext = "." + fileext
		quiet = BoolProperty(default=False, options={"HIDDEN"})
		filter_glob = StringProperty(default="*." + fileext, options={'HIDDEN'})
		
		def execute(self, context):
			end_posemode()
			end_editmode()
			sel = store_selection()
			s = "from KrxImpExp" + " import " + expname
			exec(s)			
			s = expname + "." + expname + "('" + self.filepath.replace("\\", "\\\\") + "', " + str(self.quiet) + ")"
			exec(s)
			restore_selection(sel)
			return {'FINISHED'}
	
	bpy.utils.register_class(__PrivateOperator)
	
	def __menu_func_export(self, context):
		self.layout.operator(__PrivateOperator.bl_idname, text = description + " (." + fileext + ")")
	
	bpy.types.INFO_MT_file_export.append(__menu_func_export)
	
	global __registered_exporters
	__registered_exporters[expname] = [__menu_func_export, __PrivateOperator]	

# Unregisters an exporter
def unregister_exporter(expname):
	global __registered_exporters
	bpy.types.INFO_MT_file_export.remove(__registered_exporters[expname][0])
	bpy.utils.unregister_class(__registered_exporters[expname][1])
	del __registered_exporters[expname]

# Call an importer
def call_importer(impname, filepath, quiet = False):
	s = "bpy.ops.import_scene." + impname.lower() + "(filepath = '" + filepath.replace("\\", "\\\\") + "', quiet = " + str(quiet) + ")"
	exec(s)
	
# Call an exporter
def call_exporter(expname, filepath, quiet = False):
	s = "bpy.ops.export_scene." + expname.lower() + "(filepath = '" + filepath.replace("\\", "\\\\") + "', quiet = " + str(quiet) + ")"
	exec(s)
