import bpy
import sys

argv = sys.argv
argv = argv[argv.index("--") + 1:]

for obj in bpy.data.objects:
    obj.select = not obj.name.upper().startswith('LEVEL')
bpy.ops.object.delete()

bpy.ops.export_scene.krx3dsexp(filepath=argv[0])