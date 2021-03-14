# __init__.py: Initialisation.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------

bl_info = {
 	"name": "KrxImpExp", 
	"author": "Kerrax (kerrax@mail.ru)", 
	"version": (1, 0), 
	"blender": (2, 5, 6), 
	"api": 34076, 
	"location": "File > Import-Export", 
	"description": "Plugins written by Kerrax for G1-G2a modding", 
	"category": "Import-Export"} 

# Import required modules
from KrxImpExp import image_search

# Comment a line of the following to disable an importer/exporter
from KrxImpExp import Krx3dsImp
from KrxImpExp import Krx3dsExp
from KrxImpExp import KrxAscImp
from KrxImpExp import KrxAscExp
from KrxImpExp import KrxMshImp
from KrxImpExp import KrxMrmImp
from KrxImpExp import KrxZenImp
	
# Register
def register():
	if "image_search" in globals():
		image_search.register()
	
	if "Krx3dsImp" in globals():
		Krx3dsImp.register()
	
	if "Krx3dsExp" in globals():
		Krx3dsExp.register()
	
	if "KrxAscImp" in globals():
		KrxAscImp.register()
	
	if "KrxAscExp" in globals():
		KrxAscExp.register()
	
	if "KrxMshImp" in globals():
		KrxMshImp.register()
	
	if "KrxMrmImp" in globals():
		KrxMrmImp.register()
	
	if "KrxZenImp" in globals():
		KrxZenImp.register()
	
# Unregister
def unregister():
	if "image_search" in globals():
		image_search.unregister()
	
	if "Krx3dsImp" in globals():
		Krx3dsImp.unregister()
	
	if "Krx3dsExp" in globals():
		Krx3dsExp.unregister()
	
	if "KrxAscImp" in globals():
		KrxAscImp.unregister()
	
	if "KrxAscExp" in globals():
		KrxAscExp.unregister()
	
	if "KrxMshImp" in globals():
		KrxMshImp.unregister()
	
	if "KrxMrmImp" in globals():
		KrxMrmImp.unregister()
	
	if "KrxZenImp" in globals():
		KrxZenImp.unregister()

if __name__ == "__main__":
	register()
