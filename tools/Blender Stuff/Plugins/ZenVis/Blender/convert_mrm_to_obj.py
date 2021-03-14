import bpy
import sys

argv = sys.argv
argv = argv[argv.index("--") + 1:]

bpy.ops.import_scene.krxmrmimp(filepath=argv[1])
for obj in bpy.data.objects:
    obj.select = (obj.type == 'MESH')
    bpy.context.scene.objects.active = obj
bpy.ops.object.join()
bpy.ops.export_scene.obj(filepath=argv[2], check_existing=False, path_mode='STRIP')