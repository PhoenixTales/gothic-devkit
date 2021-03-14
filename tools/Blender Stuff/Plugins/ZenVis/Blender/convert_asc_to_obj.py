import bpy
import sys

argv = sys.argv
argv = argv[argv.index("--") + 1:]
meshCount = 0

bpy.ops.import_scene.krxascimp(filepath=argv[0])

for obj in bpy.data.objects:
    if (obj.name.startswith('ZS_')) or (obj.name == 'Armature'):
        obj.select = True
    elif (obj.type == 'MESH'):
        meshCount += 1

bpy.ops.object.delete()
bpy.context.scene.objects.active == None

if (meshCount > 1):
    for obj in bpy.data.objects:
        if (obj.type == 'MESH'):
            obj.select = True
            bpy.context.scene.objects.active = obj
    bpy.ops.object.join()

for obj in bpy.data.objects:
    if (obj.type == 'MESH'):
        obj.location[0] -= float(argv[2])
        obj.location[1] -= float(argv[3])
        obj.location[2] -= float(argv[4])

bpy.ops.export_scene.obj(filepath=argv[1], check_existing=False, path_mode='STRIP')