import bpy
import mathutils
import json
import sys

class Vector3:
    def __init__(self, x, y, z):
        self.X = x
        self.Y = y
        self.Z = z

class zMAT3:
    def __init__(self, matrix):
        self.m = [[0 for x in range(3)] for y in range(3)]
        self.m[0][0] = matrix[0][0]
        self.m[0][1] = matrix[0][1]
        self.m[0][2] = matrix[0][2]
        self.m[2][0] = matrix[1][0]
        self.m[2][1] = matrix[1][1]
        self.m[2][2] = matrix[1][2]
        self.m[1][0] = matrix[2][0]
        self.m[1][1] = matrix[2][1]
        self.m[1][2] = matrix[2][2]

class Visual:
    def __init__(self, obj):
        self.FileName = obj.name.upper()
        self.Position = Vector3(obj.location[0] * 100.0, obj.location[2] * 100.0, obj.location[1] * 100.0).__dict__
        self.Scale = Vector3(obj.scale[0], obj.scale[1], obj.scale[2]).__dict__
        obj.scale = mathutils.Vector((-1, -1, -1))
        bpy.context.scene.update()
        self.Rotation = zMAT3(obj.matrix_world.to_3x3()).__dict__

argv = sys.argv
argv = argv[argv.index("--") + 1:]

vis = list()

for obj in bpy.data.objects:
    if (obj.type == 'MESH'):
        vis.append(Visual(obj).__dict__)

with open(argv[0], 'w') as file:
    json.dump(vis, file)