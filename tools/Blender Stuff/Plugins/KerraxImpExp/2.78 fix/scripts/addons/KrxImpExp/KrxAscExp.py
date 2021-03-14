
from KrxImpExp.impexp import *

class TNameAnalyzer:
	def Init(self):
		self. __fullName = ("")
		self. __prefix = ("")
		self. __shortName = ("")
		self. __objType = (0)

	def Analyze(self, fullName):
		self. Init()
		self. __fullName = (fullName)
		self. __shortName = (fullName)
		
		fullLen = (len(fullName))
		for i in range((0), (fullLen - 2)):
			s1 = ((fullName)[i:i + 1])
			if((s1 == "Z") or (s1 == "z")):
				s2 = ((fullName)[i + 1:i + 2])
				if((s2 == "S") or (s2 == "s")):
					s3 = ((fullName)[i + 2:i + 3])
					if(s3 == "_"):
						self. __prefix = ((fullName)[0:i])
						self. __shortName = ((fullName)[i:fullLen])
						self. __objType = (2)
						return

		for i in range((0), (fullLen - 2)):
			s1 = ((fullName)[i:i + 1])
			if((s1 == "Z") or (s1 == "z")):
				s2 = ((fullName)[i + 1:i + 2])
				if((s2 == "M") or (s2 == "m")):
					s3 = ((fullName)[i + 2:i + 3])
					if(s3 == "_"):
						self. __prefix = ((fullName)[0:i])
						self. __shortName = ((fullName)[i:fullLen])
						self. __objType = (3)
						return

		for i in range((0), (fullLen - 4)):
			s1 = ((fullName)[i:i + 1])
			if((s1 == "B") or (s1 == "b")):
				s2 = ((fullName)[i + 1:i + 2])
				if((s2 == "I") or (s2 == "i")):
					s3 = ((fullName)[i + 2:i + 3])
					if((s3 == "P") or (s3 == "p")):
						s4 = ((fullName)[i + 3:i + 4])
						if(s4 == "0"):
							s5 = ((fullName)[i + 4:i + 5])
							if(s5 == "1"):
								self. __prefix = ((fullName)[0:i])
								self. __shortName = ((fullName)[i:fullLen])
								self. __objType = (1)

	def GetObjectType(self):
		return (self. __objType)

	def GetFullName(self):
		return (self. __fullName)

	def GetShortName(self):
		return (self. __shortName)

	def GetPrefix(self):
		return (self. __prefix)

def NewNameAnalyzer():
	namAnl = (TNameAnalyzer())
	namAnl.Init()
	return (namAnl)

def AnalyzeName(fullName):
	namAnl = (NewNameAnalyzer())
	namAnl.Analyze(fullName)
	return (namAnl)

def FormatMsg(fmt, args):
	msg = (fmt)
	for i in range((0), (9)):
		argtempl = ("%" + (int_to_string(i + 1)))
		argpos = ((msg).find(argtempl))
		if(argpos != -1):
			msg = ((msg)[0:argpos] + ((args)[i]) + (msg)[argpos + 2:len(msg)])

	return (msg)

def FormatMsg0(fmt):
	args = ([])
	return (FormatMsg(fmt, args))

def FormatMsg1(fmt, arg1):
	args = ([])
	args.append(arg1)
	return (FormatMsg(fmt, args))

def FormatMsg2(fmt, arg1, arg2):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	return (FormatMsg(fmt, args))

def FormatMsg3(fmt, arg1, arg2, arg3):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	return (FormatMsg(fmt, args))

def FormatMsg4(fmt, arg1, arg2, arg3, arg4):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	args.append(arg4)
	return (FormatMsg(fmt, args))

def FormatMsg5(fmt, arg1, arg2, arg3, arg4, arg5):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	args.append(arg4)
	args.append(arg5)
	return (FormatMsg(fmt, args))

def FormatMsg6(fmt, arg1, arg2, arg3, arg4, arg5, arg6):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	args.append(arg4)
	args.append(arg5)
	args.append(arg6)
	return (FormatMsg(fmt, args))

class TFile:
	def __MoveFilePos(self, ofs):
		self. __pos = (self. __pos + ofs)
		if(self. __size < self. __pos):
			self. __size = (self. __pos)

	def Init(self):
		self. __stream = (None)
		self. __name = ("")
		self. __mode = ("")
		self. __size = (0)
		self. __pos = (0)

	def Open(self, filename, mode):
		if(self. __stream != None):
			self. Close()
		
		filesz = (get_file_size(filename))
		self. __stream = (open_file(filename, mode))
		if(self. __stream == None):
			cr = ((mode).find("w") != -1)
			if(cr):
				raise RuntimeError(FormatMsg1("Could not open file for write.\nFile path: \"%1\".", filename))
			else:
				raise RuntimeError(FormatMsg1("Could not open file for read.\nFile path: \"%1\".", filename))

		self. __name = (filename)
		self. __mode = (mode)
		self. __size = (filesz)
		self. __pos = (0)

	def IsOpened(self):
		return (self. __stream != None)

	def Close(self):
		if(self. __stream != None):
			close_file(self. __stream)
			self. Init()

	def GetName(self):
		return (self. __name)

	def GetMode(self):
		return (self. __mode)

	def GetSize(self):
		return (self. __size)

	def GetPos(self):
		return (self. __pos)

	def SetPos(self, pos):
		if(pos != self. __pos):
			if(pos < 0 or pos > self. __size):
				raise RuntimeError(FormatMsg4("Attempt to seek file pointer out of file.\nFile path: \"%1\".\nPosition of file pointer: %2.\nAllowable range: %3..%4.", self. __name, ("0x"+hex(pos)[2: ].upper()), ("0x"+hex(0)[2: ].upper()), ("0x"+hex(self. __size)[2: ].upper())))
			
			file_seek(self. __stream, pos);
			self. __pos = (pos)

	def Eof(self):
		return (self. __pos == self. __size)

	def WriteSignedChar(self, i):
		if(i < -128 or i > 127):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "-128", "127"))
		
		b = ((write_signed_char(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "1"))
		
		self. __MoveFilePos(1)

	def WriteUnsignedChar(self, i):
		if(i < 0 or i > 255):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "0", "255"))
		
		b = ((write_unsigned_char(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "1"))
		
		self. __MoveFilePos(1)

	def WriteBool(self, b):
		i = (0)
		if(b):
			i = (1)
		
		self. WriteUnsignedChar(i)

	def WriteSignedShort(self, i):
		if(i < -32768 or i > 32767):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "-32768", "32767"))
		
		b = ((write_signed_short(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "2"))
		
		self. __MoveFilePos(2)

	def WriteUnsignedShort(self, i):
		if(i < 0 or i > 65535):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "0", "65535"))
		
		b = ((write_unsigned_short(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "2"))
		
		self. __MoveFilePos(2)

	def WriteSignedLong(self, i):
		b = ((write_signed_long(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "4"))
		
		self. __MoveFilePos(4)

	def WriteUnsignedLong(self, i):
		b = ((write_unsigned_long(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "4"))
		
		self. __MoveFilePos(4)

	def WriteFloat(self, f):
		b = ((write_float(self. __stream, f)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "4"))
		
		self. __MoveFilePos(4)

	def WriteString(self, str):
		b = ((write_stringz(self. __stream, str)))
		sz = (len(str) + 1)
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, (int_to_string(sz))))
		
		self. __MoveFilePos(sz)

	def WriteLine(self, str):
		b = ((write_line(self. __stream, str)))
		sz = (len(str) + 2)
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, (int_to_string(sz))))
		
		self. __MoveFilePos(sz)

	def ReadSignedChar(self):
		i = ((read_signed_char(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "1"))
		
		self. __MoveFilePos(1)
		return (i)

	def ReadUnsignedChar(self):
		i = ((read_unsigned_char(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "1"))
		
		self. __MoveFilePos(1)
		return (i)

	def ReadBool(self):
		i = (self. ReadUnsignedChar())
		return (i != 0)

	def ReadSignedShort(self):
		i = ((read_signed_short(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "2"))
		
		self. __MoveFilePos(2)
		return (i)

	def ReadUnsignedShort(self):
		i = ((read_unsigned_short(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "2"))
		
		self. __MoveFilePos(2)
		return (i)

	def ReadSignedLong(self):
		i = ((read_signed_long(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "4"))
		
		self. __MoveFilePos(4)
		return (i)

	def ReadUnsignedLong(self):
		i = ((read_unsigned_long(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "4"))
		
		self. __MoveFilePos(4)
		return (i)

	def ReadFloat(self):
		f = ((read_float(self. __stream)))
		if(f == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "4"))
		
		self. __MoveFilePos(4)
		return (f)

	def ReadString(self):
		str = ((read_stringz(self. __stream)))
		if(str == None):
			raise RuntimeError(FormatMsg2("Could not read a null-terminated string from file.\nThe string seems to be too long.\nFile path: \"%1\".\nPosition in file: %2.", self. __name, ("0x"+hex(self. __pos)[2: ].upper())))
		
		sz = (len(str) + 1)
		self. __MoveFilePos(sz)
		return (str)

	def ReadLine(self):
		str = ((read_line(self. __stream)))
		if(str == None):
			raise RuntimeError(FormatMsg2("Could not read a CR+LF ended line from file.\nThe line seems to be too long.\nFile path: \"%1\".\nPosition in file: %2.", self. __name, ("0x"+hex(self. __pos)[2: ].upper())))
		
		self. __pos = (file_tell(self. __stream))
		return (str)

def NewFile():
	fl = (TFile())
	fl.Init()
	return (fl)

class TModelHierarchy:
	def Init(self):
		self. __modelPrefix = ("")
		self. __modelType = (0)
		self. __objects = ([])
		self. __objectParents = ([])
		self. __objectTypes = ([])

	def Write(self, f):
		f.WriteString(self. __modelPrefix)
		f.WriteUnsignedChar(self. __modelType)
		numObjects = ((len(self. __objects)))
		f.WriteUnsignedLong(numObjects)
		for i in range((0), (numObjects)):
			f.WriteString(((self. __objects)[i]))
			f.WriteString(((self. __objectParents)[i]))
			f.WriteUnsignedChar(((self. __objectTypes)[i]))

	def Read(self, f):
		self. __modelPrefix = (f.ReadString())
		self. __modelType = (f.ReadUnsignedChar())
		numObjects = (f.ReadUnsignedLong())
		self. __objects = ([])
		self. __objectParents = ([])
		self. __objectTypes = ([])
		for i in range((0), (numObjects)):
			self. __objects.append(f.ReadString())
			self. __objectParents.append(f.ReadString())
			self. __objectTypes.append(f.ReadUnsignedChar())

	def SetModelPrefix(self, modelPrefix):
		self. __modelPrefix = (modelPrefix)

	def GetModelPrefix(self):
		return (self. __modelPrefix)

	def SetModelType(self, modelType):
		self. __modelType = (modelType)

	def GetModelType(self):
		return (self. __modelType)

	def SetObjects(self, objects):
		self. __objects = (objects)

	def GetObjects(self):
		return (self. __objects)

	def SetObjectParents(self, objectParents):
		self. __objectParents = (objectParents)

	def GetObjectParents(self):
		return (self. __objectParents)

	def SetObjectTypes(self, objectTypes):
		self. __objectTypes = (objectTypes)

	def GetObjectTypes(self):
		return (self. __objectTypes)

def NewModelHierarchy():
	mh = (TModelHierarchy())
	mh.Init()
	return (mh)

class TMaterialDesc:
	def Init(self):
		self. __materialName = ("")
		self. __diffuseMapFilename = ("")
		self. __diffuseColor = (black_color())

	def GetMaterialName(self):
		return (self. __materialName)

	def SetMaterialName(self, materialName):
		self. __materialName = (materialName)

	def GetDiffuseMapFilename(self):
		return (self. __diffuseMapFilename)

	def SetDiffuseMapFilename(self, filename):
		self. __diffuseMapFilename = (filename)

	def GetDiffuseColor(self):
		return (self. __diffuseColor)

	def SetDiffuseColor(self, clr):
		self. __diffuseColor = (clr)

def NewMaterialDesc():
	m = (TMaterialDesc())
	m.Init()
	return (m)

class TMeshDesc:
	def Init(self):
		self. __verts = ([])
		self. __faces = ([])
		self. __faceMatIDs = ([])
		self. __faceSmoothGroups = ([])
		self. __edgeVis = ([])
		self. __tvFaces = ([])
		self. __tVerts = ([])

	def GetNumVerts(self):
		return ((len(self. __verts)))

	def SetNumVerts(self, numVerts):
		if(numVerts != (len(self. __verts))):
			self. __verts = ([])
			
			for j in range((0), (numVerts)):
				self. __verts.append(zero_vector())

	def GetVert(self, i):
		return (((self. __verts)[i]))

	def SetVert(self, i, pt):
		self. __verts[i] = (pt)

	def GetNumFaces(self):
		return ((len(self. __faces)))

	def SetNumFaces(self, numFaces):
		if(numFaces != (len(self. __faces))):
			self. __faces = ([])
			
			for j in range((0), (numFaces)):
				self. __faces.append(new_face(0, 0, 0))
			
			self. __faceMatIDs = ([])
			
			for j in range((0), (numFaces)):
				self. __faceMatIDs.append(0)
			
			self. __faceSmoothGroups = ([])
			
			for j in range((0), (numFaces)):
				self. __faceSmoothGroups.append(0)
			
			self. __edgeVis = ([])
			
			for j in range((0), (numFaces)):
				arr3 = ([])
				
				arr3.append(True)
				arr3.append(True)
				arr3.append(True)
				self. __edgeVis.append(arr3)

	def GetFace(self, faceIndex):
		return (((self. __faces)[faceIndex]))

	def SetFace(self, faceIndex, f):
		self. __faces[faceIndex] = (f)

	def GetFaceMatID(self, faceIndex):
		return (((self. __faceMatIDs)[faceIndex]))

	def SetFaceMatID(self, faceIndex, matID):
		self. __faceMatIDs[faceIndex] = (matID)

	def GetFaceSmoothGroup(self, faceIndex):
		return (((self. __faceSmoothGroups)[faceIndex]))

	def SetFaceSmoothGroup(self, faceIndex, smGroup):
		self. __faceSmoothGroups[faceIndex] = (smGroup)

	def GetEdgeVis(self, faceIndex, edgeIndex):
		return (((((self. __edgeVis)[faceIndex]))[edgeIndex]))

	def SetEdgeVis(self, faceIndex, edgeIndex, vis):
		((self. __edgeVis)[faceIndex])[edgeIndex] = (vis)

	def GetNumTVerts(self):
		return ((len(self. __tVerts)))

	def SetNumTVerts(self, numTVerts):
		if(numTVerts != (len(self. __tVerts))):
			self. __tVerts = ([])
			
			for j in range((0), (numTVerts)):
				self. __tVerts.append(zero_uvvert())

	def GetTVert(self, i):
		return (((self. __tVerts)[i]))

	def SetTVert(self, i, uv):
		self. __tVerts[i] = (uv)

	def GetNumTVFaces(self):
		return ((len(self. __tvFaces)))

	def SetNumTVFaces(self, numTVFaces):
		if(numTVFaces != (len(self. __tvFaces))):
			self. __tvFaces = ([])
			
			for j in range((0), (numTVFaces)):
				self. __tvFaces.append(new_tvface(0, 0, 0))

	def GetTVFace(self, i):
		return (((self. __tvFaces)[i]))

	def SetTVFace(self, i, tf):
		self. __tvFaces[i] = (tf)

def NewMeshDesc():
	m = (TMeshDesc())
	m.Init()
	return (m)

class TSoftSkinVert:
	def Init(self):
		self. __bones = ([])
		self. __weights = ([])

	def SetNumWeights(self, numWeights):
		if((len(self. __bones)) != numWeights):
			self. __bones = ([])
			self. __weights = ([])
			for i in range((0), (numWeights)):
				self. __bones.append("")
				self. __weights.append(0)

	def GetNumWeights(self):
		return ((len(self. __bones)))

	def SetBoneName(self, vertBoneIndex, boneName):
		self. __bones[vertBoneIndex] = (boneName)

	def GetBoneName(self, vertBoneIndex):
		return (((self. __bones)[vertBoneIndex]))

	def SetWeight(self, vertBoneIndex, weight):
		self. __weights[vertBoneIndex] = (weight)

	def GetWeight(self, vertBoneIndex):
		return (((self. __weights)[vertBoneIndex]))

def NewSoftSkinVert():
	sv = (TSoftSkinVert())
	sv.Init()
	return (sv)

class TSoftSkinVerts:
	def Init(self):
		self. __verts = ([])

	def SetNumVerts(self, numVerts):
		if((len(self. __verts)) != numVerts):
			self. __verts = ([])
			for i in range((0), (numVerts)):
				self. __verts.append(NewSoftSkinVert())

	def GetNumVerts(self):
		return ((len(self. __verts)))

	def SetVert(self, vertIndex, sv):
		self. __verts[vertIndex] = (sv)

	def GetVert(self, vertIndex):
		return (((self. __verts)[vertIndex]))

def NewSoftSkinVerts():
	svs = (TSoftSkinVerts())
	svs.Init()
	return (svs)

class TPosTrack:
	def Init(self):
		self. __samplePositions = ([])

	def SetNumSamples(self, numSamples):
		if((len(self. __samplePositions)) != numSamples):
			self. __samplePositions = ([])
			for i in range((0), (numSamples)):
				self. __samplePositions.append(zero_vector())

	def GetNumSamples(self):
		return ((len(self. __samplePositions)))

	def SetSamplePos(self, sampleIdx, pos):
		self. __samplePositions[sampleIdx] = (pos)

	def GetSamplePos(self, sampleIdx):
		return (((self. __samplePositions)[sampleIdx]))

def NewPosTrack():
	tr = (TPosTrack())
	tr.Init()
	return (tr)

class TRotTrack:
	def Init(self):
		self. __sampleAxes = ([])
		self. __sampleAngles = ([])

	def SetNumSamples(self, numSamples):
		if((len(self. __sampleAxes)) != numSamples):
			self. __sampleAxes = ([])
			self. __sampleAngles = ([])
			for i in range((0), (numSamples)):
				self. __sampleAxes.append(zero_vector())
				self. __sampleAngles.append(0)

	def GetNumSamples(self):
		return ((len(self. __sampleAxes)))

	def SetSampleAxis(self, sampleIdx, axis):
		self. __sampleAxes[sampleIdx] = (axis)

	def GetSampleAxis(self, sampleIdx):
		return (((self. __sampleAxes)[sampleIdx]))

	def SetSampleAngle(self, sampleIdx, angle):
		self. __sampleAngles[sampleIdx] = (angle)

	def GetSampleAngle(self, sampleIdx):
		return (((self. __sampleAngles)[sampleIdx]))

def NewRotTrack():
	tr = (TRotTrack())
	tr.Init()
	return (tr)

class TMorphTrack:
	def Init(self):
		self. __sampleVerts = ([])

	def SetNumSamples(self, numSamples):
		if((len(self. __sampleVerts)) != numSamples):
			self. __sampleVerts = ([])
			for j in range((0), (numSamples)):
				self. __sampleVerts.append([])

	def GetNumSamples(self):
		return ((len(self. __sampleVerts)))

	def SetSampleVerts(self, sampleIdx, verts):
		self. __sampleVerts[sampleIdx] = (verts)

	def GetSampleVerts(self, sampleIdx):
		return (((self. __sampleVerts)[sampleIdx]))

def NewMorphTrack():
	tr = (TMorphTrack())
	tr.Init()
	return (tr)

class TObjectDesc:
	def Init(self):
		self. __objectName = ("")
		self. __objectType = (0)
		self. __parentName = ("")
		self. __transform = (identity_matrix())
		self. __matRef = (0)
		self. __meshDesc = (NewMeshDesc())
		self. __ssv = (NewSoftSkinVerts())
		self. __posTrack = (NewPosTrack())
		self. __rotTrack = (NewRotTrack())
		self. __morphTrack = (NewMorphTrack())

	def GetObjectName(self):
		return (self. __objectName)

	def SetObjectName(self, objectName):
		self. __objectName = (objectName)

	def GetObjectType(self):
		return (self. __objectType)

	def SetObjectType(self, objectType):
		self. __objectType = (objectType)

	def GetParentName(self):
		return (self. __parentName)

	def SetParentName(self, parentName):
		self. __parentName = (parentName)

	def GetTransform(self):
		a00 = (get_x(get_row0(self. __transform)))
		a01 = (get_y(get_row0(self. __transform)))
		a02 = (get_z(get_row0(self. __transform)))
		a10 = (get_x(get_row1(self. __transform)))
		a11 = (get_y(get_row1(self. __transform)))
		a12 = (get_z(get_row1(self. __transform)))
		a20 = (get_x(get_row2(self. __transform)))
		a21 = (get_y(get_row2(self. __transform)))
		a22 = (get_z(get_row2(self. __transform)))
		a30 = (get_x(get_row3(self. __transform)))
		a31 = (get_y(get_row3(self. __transform)))
		a32 = (get_z(get_row3(self. __transform)))
		transformCopy = (new_matrix(new_vector(a00, a01, a02), new_vector(a10, a11, a12), new_vector(a20, a21, a22), new_vector(a30, a31, a32)))
		return (transformCopy)

	def SetTransform(self, transform):
		self. __transform = (transform)

	def GetMaterialRef(self):
		return (self. __matRef)

	def SetMaterialRef(self, matRef):
		self. __matRef = (matRef)

	def GetMeshDesc(self):
		return (self. __meshDesc)

	def SetMeshDesc(self, meshDesc):
		self. __meshDesc = (meshDesc)

	def GetPosTrack(self):
		return (self. __posTrack)

	def SetPosTrack(self, posTrack):
		self. __posTrack = (posTrack)

	def GetRotTrack(self):
		return (self. __rotTrack)

	def SetRotTrack(self, rotTrack):
		self. __rotTrack = (rotTrack)

	def GetMorphTrack(self):
		return (self. __morphTrack)

	def SetMorphTrack(self, morphTrack):
		self. __morphTrack = (morphTrack)

	def GetSoftSkinVerts(self):
		return (self. __ssv)

	def SetSoftSkinVerts(self, ssv):
		self. __ssv = (ssv)

def NewObjectDesc():
	m = (TObjectDesc())
	m.Init()
	return (m)

class TSceneAnalyzer:
	def __AppendPrefix(self, prefix, sel):
		found = (False)
		for i in range((0), ((len(self. __scenePrefixes)))):
			if(stricmp(((self. __scenePrefixes)[i]), prefix) == 0):
				found = (True)

		if( not (found)):
			self. __scenePrefixes.append(prefix)
		
		if(sel):
			found = (False)
			for i in range((0), ((len(self. __selectedPrefixes)))):
				if(stricmp(((self. __selectedPrefixes)[i]), prefix) == 0):
					found = (True)

			if( not (found)):
				self. __selectedPrefixes.append(prefix)

	def __PrepareSceneObjects(self, obj):
		if(obj != None):
			objName = ((get_object_name(obj)))
			selected = ((is_selected(obj)))
			isMeshByType = (is_mesh_object(obj))
			na = (AnalyzeName(objName))
			self. __sceneObjects.append(objName)
			if(selected):
				self. __selectedObjects.append(objName)
			
			if(isMeshByType):
				self. __sceneMeshesByType.append(objName)
				if(selected):
					self. __selectedMeshesByType.append(objName)

			prefix = (na.GetPrefix())
			objType = (na.GetObjectType())
			if(objType == 2):
				self. __sceneSlots.append(objName)
				if(selected):
					self. __selectedSlots.append(objName)
				
				self. __AppendPrefix(prefix, selected)
			elif(objType == 1):
				self. __sceneBones.append(objName)
				if(selected):
					self. __selectedBones.append(objName)
				
				self. __AppendPrefix(prefix, selected)
			elif(objType == 3):
				self. __sceneMeshes.append(objName)
				if(selected):
					self. __selectedMeshes.append(objName)
				
				self. __AppendPrefix(prefix, selected)
			else:
				self. __sceneDummies.append(objName)
				if(selected):
					self. __selectedDummies.append(objName)

		children = ((get_children(obj)))
		for i in range((0), ((len(children)))):
			self. __PrepareSceneObjects(((children)[i]))

	def __GetObjectsByPrefix(self, prefix):
		self. __objectsByPrefix = ([])
		
		conflictPrefixes = ([])
		for i in range((0), ((len(self. __scenePrefixes)))):
			a = (((self. __scenePrefixes)[i]))
			if((a != prefix) and ((a)[0:len(prefix)] == prefix)):
				conflictPrefixes.append(a)

		for i in range((0), ((len(self. __sceneObjects)))):
			objName = (((self. __sceneObjects)[i]))
			if((objName)[0:len(prefix)] == prefix):
				noConflicts = (True)
				for j in range((0), ((len(conflictPrefixes)))):
					c = (((conflictPrefixes)[j]))
					if((objName)[0:len(c)] == c):
						noConflicts = (False)
						break

				if(noConflicts):
					self. __objectsByPrefix.append(objName)

		return (self. __objectsByPrefix)

	def __PrepareModelHierarchies(self):
		self. __modelHierarchies = ([])
		
		for i in range((0), ((len(self. __scenePrefixes)))):
			prefix = (((self. __scenePrefixes)[i]))
			objectsByPrefix = (self. __GetObjectsByPrefix(prefix))
			
			numBones = (0)
			numSlots = (0)
			numMeshes = (0)
			bip01Found = (False)
			objects = (objectsByPrefix)
			objectTypes = ([])
			for j in range((0), ((len(objects)))):
				objName = (((objects)[j]))
				na = (AnalyzeName(objName))
				objType = (na.GetObjectType())
				if(objType == 1):
					numBones = (numBones + 1)
					if(stricmp(na.GetShortName(), "Bip01") == 0):
						bip01Found = (True)
					
				elif(objType == 2):
					numSlots = (numSlots + 1)
				elif(objType == 3):
					numMeshes = (numMeshes + 1)
				
				objects[j] = ((objName)[len(prefix):len(objName)])
				objectTypes.append(objType)

			modelType = (0)
			if(numMeshes == 1 and numBones == 0 and numSlots == 0):
				modelType = ((1 + 2))
			elif(bip01Found):
				modelType = ((8 + 16))
			else:
				modelType = ((4))

			if(modelType == (8 + 16)):
				rootBoneIndex = ( -1)
				for i in range((0), ((len(objects)))):
					objName = (((objects)[i]))
					objType = (((objectTypes)[i]))
					if(objType == 1):
						rootBoneIndex = (i)
						break

				if(rootBoneIndex != -1 and rootBoneIndex != 0):
					tmp = (((objects)[0]))
					objects[0] = (((objects)[rootBoneIndex]))
					objects[rootBoneIndex] = (tmp)
					tmp2 = (((objectTypes)[0]))
					objectTypes[0] = (((objectTypes)[rootBoneIndex]))
					objectTypes[rootBoneIndex] = (tmp2)
				
				withoutLeadDummies = ([])
				withoutLeadDummies2 = ([])
				for i in range((0), ((len(objects)))):
					objName = (((objects)[i]))
					objType = (((objectTypes)[i]))
					if((objType != 0) or (i > rootBoneIndex)):
						withoutLeadDummies.append(objName)
						withoutLeadDummies2.append(objType)

				objects = (withoutLeadDummies)
				objectTypes = (withoutLeadDummies2)

			objectParents = ([])
			for j in range((0), ((len(objects)))):
				defaultParentName = ("")
				if((modelType == (8 + 16)) and (j > 0)):
					defaultParentName = (((objects)[0]))
				
				objName = (((objects)[j]))
				obj = ((find_object_by_name(prefix + objName)))
				parentName = (defaultParentName)
				if(obj != None):
					parent = ((get_parent(obj)))
					if(parent != None):
						tmp = ((get_object_name(parent)))
						for k in range((0), (j)):
							if(tmp == prefix + ((objects)[k])):
								parentName = (((objects)[k]))
								break

				objectParents.append(parentName)

			mh = (NewModelHierarchy())
			mh.SetModelPrefix(prefix)
			mh.SetModelType(modelType)
			mh.SetObjects(objects)
			mh.SetObjectParents(objectParents)
			mh.SetObjectTypes(objectTypes)
			self. __modelHierarchies.append(mh)

	def __GetModelHierarchyByPrefix(self, prefix):
		for i in range((0), ((len(self. __modelHierarchies)))):
			mh = (((self. __modelHierarchies)[i]))
			if(mh.GetModelPrefix() == prefix):
				return (mh)

		return (((self. __modelHierarchies)[ -1]))

	def __CheckPrefixForUnique(self, prefix):
		for i in range((0), ((len(self. __scenePrefixes)))):
			if(((self. __scenePrefixes)[i]) == prefix):
				return (False)

		return (True)

	def __PrepareUniquePrefix(self):
		prefix = ("")
		a = ((ord("A")))
		z = ((ord("Z")))
		codes = ([])
		
		while( not (self. __CheckPrefixForUnique(prefix))):
			pos = ((len(codes)) - 1)
			while(True):
				if(pos == -1):
					for i in range((0), ((len(codes)))):
						codes[i] = (a)
					
					codes.append(a)
					break
				elif(((codes)[pos]) < z):
					codes[pos] = (((codes)[pos]) + 1)
					break
				else:
					codes[pos] = (a)
					pos = (pos - 1)

			prefix = ("")
			for i in range((0), ((len(codes)))):
				prefix = (prefix + (chr(((codes)[i]))))
			
			if(len(prefix) != 0):
				prefix = (prefix + " ")

		self. __uniquePrefix = (prefix)

	def __Init(self):
		self. __sceneObjects = ([])
		self. __selectedObjects = ([])
		self. __sceneMeshesByType = ([])
		self. __selectedMeshesByType = ([])
		self. __sceneSlots = ([])
		self. __selectedSlots = ([])
		self. __sceneBones = ([])
		self. __selectedBones = ([])
		self. __sceneMeshes = ([])
		self. __selectedMeshes = ([])
		self. __sceneDummies = ([])
		self. __selectedDummies = ([])
		self. __scenePrefixes = ([])
		self. __selectedPrefixes = ([])
		self. __uniquePrefix = ("")
		self. __objectsByPrefix = ([])
		self. __modelHierarchies = ([])
		self. __appropriatePrefixes = ([])
		self. __selectedAppropriatePrefixes = ([])

	def __Analyze(self):
		show_progress_bar("Analyzing scene", 1)
		self. __Init()
		self. __PrepareSceneObjects(None)
		self. __PrepareModelHierarchies()
		self. __PrepareUniquePrefix()

	def __FindAppropriateDynamicModels(self, objectsDesc):
		self. __appropriatePrefixes = ([])
		self. __selectedAppropriatePrefixes = ([])
		for i in range((0), ((len(self. __scenePrefixes)))):
			prefix = (((self. __scenePrefixes)[i]))
			appropriate = (True)
			
			for j in range((0), ((len(objectsDesc)))):
				objName = (((objectsDesc)[j]).GetObjectName())
				na = (AnalyzeName(objName))
				objType = (na.GetObjectType())
				if((objType == 1) or (objType == 2)):
					matchFound = (False)
					if(objType == 1):
						for k in range((0), ((len(self. __sceneBones)))):
							if(stricmp(((self. __sceneBones)[k]), prefix + objName) == 0):
								matchFound = (True)
								break

					else:
						for k in range((0), ((len(self. __sceneSlots)))):
							if(stricmp(((self. __sceneSlots)[k]), prefix + objName) == 0):
								matchFound = (True)
								break

					if( not (matchFound)):
						appropriate = (False)
						break

			if(appropriate):
				self. __appropriatePrefixes.append(prefix)
				for j in range((0), ((len(self. __selectedPrefixes)))):
					if(((self. __selectedPrefixes)[j]) == prefix):
						self. __selectedAppropriatePrefixes.append(prefix)
						break

	def __FindAppropriateMorphMeshes(self, objectsDesc):
		self. __appropriatePrefixes = ([])
		self. __selectedAppropriatePrefixes = ([])
		for i in range((0), ((len(self. __scenePrefixes)))):
			prefix = (((self. __scenePrefixes)[i]))
			appropriate = (True)
			
			for j in range((0), ((len(objectsDesc)))):
				objName = (((objectsDesc)[j]).GetObjectName())
				na = (AnalyzeName(objName))
				objType = (na.GetObjectType())
				if(objType == 3):
					matchFound = (False)
					for k in range((0), ((len(self. __sceneMeshes)))):
						if(((self. __sceneMeshes)[k]) == prefix + objName):
							obj = ((find_object_by_name(prefix + objName)))
							if(is_mesh_object(obj)):
								msh = (MeshData(obj))
								numVerts1 = ((msh.get_num_verts()))
								morphTrack = (((objectsDesc)[j]).GetMorphTrack())
								if(morphTrack.GetNumSamples() > 0):
									numVerts2 = ((len(morphTrack.GetSampleVerts(0))))
									if(numVerts1 == numVerts2):
										matchFound = (True)
										break

					if( not (matchFound)):
						appropriate = (False)
						break

			if(appropriate):
				self. __appropriatePrefixes.append(prefix)
				for j in range((0), ((len(self. __selectedPrefixes)))):
					if(((self. __selectedPrefixes)[j]) == prefix):
						self. __selectedAppropriatePrefixes.append(prefix)
						break

	def Init(self):
		self. __Init()

	def Analyze(self):
		self. __Analyze()

	def GetSceneObjects(self):
		return (self. __sceneObjects)

	def GetSelectedObjects(self):
		return (self. __selectedObjects)

	def GetSceneMeshesByType(self):
		return (self. __sceneMeshesByType)

	def GetSelectedMeshesByType(self):
		return (self. __selectedMeshesByType)

	def GetSceneSlots(self):
		return (self. __sceneSlots)

	def GetSelectedSlots(self):
		return (self. __selectedSlots)

	def GetSceneBones(self):
		return (self. __sceneBones)

	def GetSelectedBones(self):
		return (self. __selectedBones)

	def GetSceneMeshes(self):
		return (self. __sceneMeshes)

	def GetSelectedMeshes(self):
		return (self. __selectedMeshes)

	def GetSceneDummies(self):
		return (self. __sceneDummies)

	def GetSelectedDummies(self):
		return (self. __selectedDummies)

	def GetScenePrefixes(self):
		return (self. __scenePrefixes)

	def GetSelectedPrefixes(self):
		return (self. __selectedPrefixes)

	def GetUniquePrefix(self):
		return (self. __uniquePrefix)

	def GetModelHierarchies(self):
		return (self. __modelHierarchies)

	def GetModelHierarchyByPrefix(self, prefix):
		return (self. __GetModelHierarchyByPrefix(prefix))

	def FindAppropriateDynamicModels(self, objectsDesc):
		self. __FindAppropriateDynamicModels(objectsDesc)

	def FindAppropriateMorphMeshes(self, objectsDesc):
		self. __FindAppropriateMorphMeshes(objectsDesc)

	def GetAppropriatePrefixes(self):
		return (self. __appropriatePrefixes)

	def GetSelectedAppropriatePrefixes(self):
		return (self. __selectedAppropriatePrefixes)

def NewSceneAnalyzer():
	sa = (TSceneAnalyzer())
	sa.Init()
	return (sa)

def AnalyzeScene():
	sa = (NewSceneAnalyzer())
	sa.Analyze()
	return (sa)

class TSpaceTransform:
	def Init(self):
		self. __setupUnit = (5)
		self. __systemUnitsPerFileUnit = (1)
		self. __fileUnitsPerSystemUnit = (1)

	def Write(self, f):
		f.WriteSignedLong(self. __setupUnit)
		f.WriteFloat(self. __systemUnitsPerFileUnit)
		f.WriteFloat(self. __fileUnitsPerSystemUnit)

	def Read(self, f):
		self. __setupUnit = (f.ReadSignedLong())
		self. __systemUnitsPerFileUnit = (f.ReadFloat())
		self. __fileUnitsPerSystemUnit = (f.ReadFloat())

	def GetSetupUnit(self):
		return (self. __setupUnit)

	def SetSetupUnit(self, setupUnit):
		self. __setupUnit = (setupUnit)

	def GetSystemUnitsPerFileUnit(self):
		return (self. __systemUnitsPerFileUnit)

	def SetSystemUnitsPerFileUnit(self, scaleCoef):
		self. __systemUnitsPerFileUnit = (scaleCoef)
		self. __fileUnitsPerSystemUnit = ((int_to_float(1)) / scaleCoef)

	def GetFileUnitsPerSystemUnit(self):
		return (self. __fileUnitsPerSystemUnit)

	def SetFileUnitsPerSystemUnit(self, scaleCoef):
		self. __fileUnitsPerSystemUnit = (scaleCoef)
		self. __systemUnitsPerFileUnit = ((int_to_float(1)) / scaleCoef)

def NewSpaceTransform():
	st = (TSpaceTransform())
	st.Init()
	return (st)

class TTimeTransform:
	def Init(self):
		self. __startFrameInFile = (0)
		self. __endFrameInFile = (100)
		self. __startFrameInScene = (0)
		self. __endFrameInScene = (100)
		self. __minFrameInFile = (0)
		self. __maxFrameInFile = (100)
		self. __minFrameInScene = (0)
		self. __maxFrameInScene = (100)

	def Write(self, f):
		f.WriteSignedLong(self. __startFrameInFile)
		f.WriteSignedLong(self. __endFrameInFile)
		f.WriteSignedLong(self. __startFrameInScene)
		f.WriteSignedLong(self. __endFrameInScene)
		f.WriteSignedLong(self. __minFrameInFile)
		f.WriteSignedLong(self. __maxFrameInFile)
		f.WriteSignedLong(self. __minFrameInScene)
		f.WriteSignedLong(self. __maxFrameInScene)

	def Read(self, f):
		self. __startFrameInFile = (f.ReadSignedLong())
		self. __endFrameInFile = (f.ReadSignedLong())
		self. __startFrameInScene = (f.ReadSignedLong())
		self. __endFrameInScene = (f.ReadSignedLong())
		self. __minFrameInFile = (f.ReadSignedLong())
		self. __maxFrameInFile = (f.ReadSignedLong())
		self. __minFrameInScene = (f.ReadSignedLong())
		self. __maxFrameInScene = (f.ReadSignedLong())

	def GetStartFrameInFile(self):
		return (self. __startFrameInFile)

	def SetStartFrameInFile(self, frameIndex):
		self. __startFrameInFile = (frameIndex)

	def GetEndFrameInFile(self):
		return (self. __endFrameInFile)

	def SetEndFrameInFile(self, frameIndex):
		self. __endFrameInFile = (frameIndex)

	def GetStartFrameInScene(self):
		return (self. __startFrameInScene)

	def SetStartFrameInScene(self, frameIndex):
		self. __startFrameInScene = (frameIndex)

	def GetEndFrameInScene(self):
		return (self. __endFrameInScene)

	def SetEndFrameInScene(self, frameIndex):
		self. __endFrameInScene = (frameIndex)

	def GetMinFrameInFile(self):
		return (self. __minFrameInFile)

	def SetMinFrameInFile(self, frameIndex):
		self. __minFrameInFile = (frameIndex)

	def GetMaxFrameInFile(self):
		return (self. __maxFrameInFile)

	def SetMaxFrameInFile(self, frameIndex):
		self. __maxFrameInFile = (frameIndex)

	def GetMinFrameInScene(self):
		return (self. __minFrameInScene)

	def SetMinFrameInScene(self, frameIndex):
		self. __minFrameInScene = (frameIndex)

	def GetMaxFrameInScene(self):
		return (self. __maxFrameInScene)

	def SetMaxFrameInScene(self, frameIndex):
		self. __maxFrameInScene = (frameIndex)

def NewTimeTransform():
	tt = (TTimeTransform())
	tt.Init()
	return (tt)

def GetDefaultMaterialName(obj):
	clr = ((get_wire_color(obj)))
	ri = ((float_to_int(get_red(clr) * 255)))
	gi = ((float_to_int(get_green(clr) * 255)))
	bi = ((float_to_int(get_blue(clr) * 255)))
	mtlName = ("R" + (int_to_string(ri)) + " G" + (int_to_string(gi)) + " B" + (int_to_string(bi)))
	return (mtlName)

def GetDefaultMaterial(obj):
	clr = ((get_wire_color(obj)))
	mtlName = (GetDefaultMaterialName(obj))
	mtlDesc = (NewMaterialDesc())
	mtlDesc.SetMaterialName(mtlName)
	mtlDesc.SetDiffuseColor(clr)
	mtlDesc.SetDiffuseMapFilename("")
	return (mtlDesc)

class TASCFileSaver:
	def __WriteLine(self, str):
		self. __file.WriteLine(str)

	def __WriteASCVersion(self):
		self. __WriteLine("*3DSMAX_ASCIIEXPORT\t110")

	def __WriteComment(self):
		self. __WriteLine("*COMMENT \"[ Kerrax ASCII Model Exporter ] - " + "Feb 25 2013" + "\"")

	def __WriteSceneInfo(self):
		caption = ("Writing scene information")
		show_progress_bar(caption, 1)
		self. __WriteLine("*SCENE {")
		firstFrame = (0)
		lastFrame = (100)
		frameSpeed = (25)
		ticksPerFrame = (192)
		if(self. __exportAnimation):
			firstFrame = (self. __timeTransform.GetStartFrameInFile())
			lastFrame = (self. __timeTransform.GetEndFrameInFile())
			frameSpeed = (get_fps())
			ticksPerFrame = ((4800 / get_fps()))
		
		self. __WriteLine("\t*SCENE_FILENAME \"" + get_scene_filename() + "\"")
		self. __WriteLine("\t*SCENE_FIRSTFRAME " + (int_to_string(firstFrame)))
		self. __WriteLine("\t*SCENE_LASTFRAME " + (int_to_string(lastFrame)))
		self. __WriteLine("\t*SCENE_FRAMESPEED " + (int_to_string(frameSpeed)))
		self. __WriteLine("\t*SCENE_TICKSPERFRAME " + (int_to_string(ticksPerFrame)))
		self. __WriteLine("\t*SCENE_BACKGROUND_STATIC 0\t0\t0")
		self. __WriteLine("\t*SCENE_AMBIENT_STATIC 0.2\t0.2\t0.2")
		self. __WriteLine("}")

	def __ColorToString(self, clr):
		return ((float_to_string(get_red(clr))) + "\t" + (float_to_string(get_green(clr))) + "\t" + (float_to_string(get_blue(clr))))

	def __EnumerateMaterials(self):
		for i in range((0), ((len(self. __objShortNames)))):
			if(((self. __objSelectFlags)[i])):
				self. __curObjShortName = (((self. __objShortNames)[i]))
				self. __curObjName = (self. __modelPrefix + self. __curObjShortName)
				self. __curObj = ((find_object_by_name(self. __curObjName)))
				if(self. __curObj != None):
					na = (AnalyzeName(self. __curObjName))
					
					if(na.GetObjectType() == 3):
						caption = ("Writing materials")
						show_progress_bar(caption, 50 * i / (len(self. __objShortNames)))
						msh = (MeshData(self. __curObj))
						numFaces = ((msh.get_num_faces()))
						defaultMatCreated = (False)
						for j in range((0), (numFaces)):
							mat = ((msh.get_face_material(j)))
							if(mat == None):
								if( not (defaultMatCreated)):
									matDesc = (GetDefaultMaterial(self. __curObj))
									found = (False)
									for k in range((0), ((len(self. __addMtls)))):
										if(((self. __addMtls)[k]).GetMaterialName() == matDesc.GetMaterialName()):
											found = (True)
											break

									if( not (found)):
										self. __addMtls.append(matDesc)
									
									defaultMatCreated = (True)
								
							else:
								found = (False)
								for k in range((0), ((len(self. __expMtls)))):
									if(((self. __expMtls)[k]) == mat):
										found = (True)
										break

								if( not (found)):
									self. __expMtls.append(mat)

	def __WriteMaterialList(self):
		self. __EnumerateMaterials()
		totalCount = ((len(self. __expMtls)) + (len(self. __addMtls)))
		
		if(totalCount != 0):
			self. __WriteLine("*MATERIAL_LIST {")
			self. __WriteLine("\t*MATERIAL_COUNT 1")
			self. __WriteLine("\t*MATERIAL 0 {")
			self. __WriteLine("\t\t*MATERIAL_NAME \"MultiMtl #0\"")
			self. __WriteLine("\t\t*MATERIAL_CLASS \"Multi/Sub-Object\"")
			self. __WriteLine("\t\t*NUMSUBMTLS " + (int_to_string(totalCount)))
			for i in range((0), (totalCount)):
				matName = ("")
				diffuseColor = (black_color())
				diffuseMap = ("")
				if(i < (len(self. __expMtls))):
					mat = (((self. __expMtls)[i]))
					matName = (get_material_name(mat))
					diffuseColor = (get_diffuse_color(mat))
					diffuseMap = (get_diffuse_map_filename(mat))
				else:
					matDesc = (((self. __addMtls)[i - (len(self. __expMtls))]))
					matName = (matDesc.GetMaterialName())
					diffuseColor = (matDesc.GetDiffuseColor())
					diffuseMap = (matDesc.GetDiffuseMapFilename())
				
				self. __WriteLine("\t\t*SUBMATERIAL " + (int_to_string(i)) + " {")
				self. __WriteLine("\t\t\t*MATERIAL_NAME \"" + matName + "\"")
				self. __WriteLine("\t\t\t*MATERIAL_CLASS \"Standard\"")
				self. __WriteLine("\t\t\t*MATERIAL_DIFFUSE " + self. __ColorToString(diffuseColor))
				mapFileName = (diffuseMap)
				if(diffuseMap != ""):
					self. __WriteLine("\t\t\t*MAP_DIFFUSE {")
					self. __WriteLine("\t\t\t\t*BITMAP \"" + diffuseMap + "\"")
					self. __WriteLine("\t\t\t}")
				
				self. __WriteLine("\t\t}")
			
			self. __WriteLine("\t}")
			self. __WriteLine("}")

	def __Point3ToString(self, pt):
		return ((float_to_string(get_x(pt))) + "\t" + (float_to_string(get_y(pt))) + "\t" + (float_to_string(get_z(pt))))

	def __WriteNodeTM(self, tm):
		trans = (get_translation_part(tm))
		q = (get_rotation_part(tm))
		mat = (multiply_matrix_matrix(rotation_matrix(q), translation_matrix(trans)))
		self. __WriteLine("\t*NODE_TM {")
		self. __WriteLine("\t\t*NODE_NAME \"" + self. __curObjShortName + "\"")
		self. __WriteLine("\t\t*TM_ROW0 " + self. __Point3ToString(get_row0(mat)))
		self. __WriteLine("\t\t*TM_ROW1 " + self. __Point3ToString(get_row1(mat)))
		self. __WriteLine("\t\t*TM_ROW2 " + self. __Point3ToString(get_row2(mat)))
		self. __WriteLine("\t\t*TM_ROW3 " + self. __Point3ToString(get_row3(mat) * self. __scaleCoef))
		self. __WriteLine("\t\t*TM_POS " + self. __Point3ToString(trans * self. __scaleCoef))
		self. __WriteLine("\t\t*TM_ROTAXIS " + self. __Point3ToString(get_axis(q)))
		self. __WriteLine("\t\t*TM_ROTANGLE " + (float_to_string(get_angle(q))))
		self. __WriteLine("\t\t*TM_SCALE 1\t1\t1")
		self. __WriteLine("\t\t*TM_SCALEAXIS 0\t0\t0")
		self. __WriteLine("\t\t*TM_SCALEAXISANG 0\t0\t0")
		self. __WriteLine("\t}")

	def __WriteMaterialRef(self, matRef):
		if(matRef != -1):
			self. __WriteLine("\t*MATERIAL_REF " + (int_to_string(matRef)))

	def __TransformTime(self, frameInScene):
		a = (self. __timeTransform.GetStartFrameInScene())
		b = (self. __timeTransform.GetStartFrameInFile())
		frameInFile = (frameInScene - a + b)
		return (frameInFile)

	def __WriteMesh(self):
		msh = (MeshData(self. __curObj))
		numVerts = ((msh.get_num_verts()))
		numFaces = ((msh.get_num_faces()))
		
		if(is_skin_object(self. __curObj) and (self. __modelType == (8 + 16))):
			self. __WriteLine("\t*MESH_SOFTSKIN {")
			self. __skinnedObjs.append(self. __curObj)
		else:
			self. __WriteLine("\t*MESH {")
		
		t = (0)
		if((self. __modelType == (1 + 2)) and (self. __exportAnimation)):
			frameInFile = (self. __TransformTime(get_current_frame()))
			t = (frameInFile * (4800 / get_fps()))
		
		self. __WriteLine("\t\t*TIMEVALUE " + (int_to_string(t)))
		self. __WriteLine("\t\t*MESH_NUMVERTEX " + (int_to_string(numVerts)))
		self. __WriteLine("\t\t*MESH_NUMFACES " + (int_to_string(numFaces)))
		
		self. __WriteLine("\t\t*MESH_VERTEX_LIST {")
		
		transform = ((get_transform(self. __curObj)))
		if(self. __modelType == (1 + 2)):
			transform = (multiply_matrix_matrix(transform, inverse_matrix((get_transform(self. __curObj)))))
		
		caption = (FormatMsg1("Writing '%1'", self. __curObjShortName))
		for i in range((0), (numVerts)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, 10 * i / numVerts)
			
			pt = ((msh.get_vert(i)))
			pt = (multiply_vector_matrix(pt, transform) * self. __scaleCoef)
			self. __WriteLine("\t\t\t*MESH_VERTEX " + (int_to_string(i)) + "\t" + self. __Point3ToString(pt))
		
		self. __WriteLine("\t\t}")
		
		det = (determinant(transform))
		
		self. __WriteLine("\t\t*MESH_FACE_LIST {")
		caption = (FormatMsg1("Writing '%1'", self. __curObjShortName))
		for i in range((0), (numFaces)):
			if(((i) & (31)) == 0):
				show_progress_bar(caption, 10 + 20 * i / numFaces)

			fc = ((msh.get_face(i)))
			vA = (get_face_vert(fc, 0))
			vB = (get_face_vert(fc, 1))
			vC = (get_face_vert(fc, 2))
			edgeAB = (0)
			if(True):
				edgeAB = (1)
			
			edgeBC = (0)
			if(True):
				edgeBC = (1)
			
			edgeCA = (0)
			if(True):
				edgeCA = (1)

			if(det < 0):
				tmp = (vA)
				vA = (vC)
				vC = (tmp)
				tmp = (edgeAB)
				edgeAB = (edgeBC)
				edgeBC = (tmp)

			smGroup = (0)
			mat = ((msh.get_face_material(i)))
			matIndex = (0)
			if(mat == None):
				defMatName = (GetDefaultMaterialName(self. __curObj))
				for j in range((0), ((len(self. __addMtls)))):
					if(((self. __addMtls)[j]).GetMaterialName() == defMatName):
						matIndex = (j + (len(self. __expMtls)))
						break

			else:
				for j in range((0), ((len(self. __expMtls)))):
					if(((self. __expMtls)[j]) == mat):
						matIndex = (j)
						break

			str = ("\t\t\t*MESH_FACE    " + (int_to_string(i)))
			str = (str + ":    A:    " + (int_to_string(vA)))
			str = (str + " B:    " + (int_to_string(vB)))
			str = (str + " C:    " + (int_to_string(vC)))
			str = (str + " AB:    " + (int_to_string(edgeAB)))
			str = (str + " BC:    " + (int_to_string(edgeAB)))
			str = (str + " CA:    " + (int_to_string(edgeAB)))
			str = (str + "\t *MESH_SMOOTHING " + (int_to_string(smGroup)))
			str = (str + " \t*MESH_MTLID " + (int_to_string(matIndex)))
			self. __WriteLine(str)
		
		self. __WriteLine("\t\t}")
		
		numTVerts = ((msh.get_num_tverts()))
		if((numTVerts != 0) and not ((self. __modelType == (1 + 2)) and (self. __exportAnimation))):
			self. __WriteLine("\t\t*MESH_NUMTVERTEX " + (int_to_string(numTVerts)))
			self. __WriteLine("\t\t*MESH_TVERTLIST {")
			caption = (FormatMsg1("Writing '%1'", self. __curObjShortName))
			for i in range((0), (numTVerts)):
				if(((i) & (63)) == 0):
					show_progress_bar(caption, 30 + 10 * i / numTVerts)
				
				uv = ((msh.get_tvert(i)))
				u = (get_u(uv))
				v = (get_v(uv))
				str = ("\t\t\t*MESH_TVERT " + (int_to_string(i)))
				str = (str + "\t" + (float_to_string(u)))
				str = (str + "\t" + (float_to_string(v)))
				str = (str + "\t0")
				self. __WriteLine(str)
			
			self. __WriteLine("\t\t}")

		numTVFaces = ((msh.get_num_faces()))
		if((numTVerts != 0) and not ((self. __modelType == (1 + 2)) and (self. __exportAnimation))):
			self. __WriteLine("\t\t*MESH_NUMTVFACES " + (int_to_string(numTVFaces)))
			self. __WriteLine("\t\t*MESH_TFACELIST {")
			caption = (FormatMsg1("Writing '%1'", self. __curObjShortName))
			for i in range((0), (numTVFaces)):
				if(((i) & (31)) == 0):
					show_progress_bar(caption, 40 + 20 * i / numTVFaces)

				tvfc = ((msh.get_tvface(i)))
				tA = (get_tvface_vert(tvfc, 0))
				tB = (get_tvface_vert(tvfc, 1))
				tC = (get_tvface_vert(tvfc, 2))
				
				if(det < 0):
					tmp = (tA)
					tA = (tC)
					tC = (tmp)

				str = ("\t\t\t*MESH_TFACE " + (int_to_string(i)))
				str = (str + "\t" + (int_to_string(tA)))
				str = (str + "\t" + (int_to_string(tB)))
				str = (str + "\t" + (int_to_string(tC)))
				self. __WriteLine(str)
			
			self. __WriteLine("\t\t}")

		self. __WriteLine("\t}")

	def __WriteTMAnimation(self, objIndex):
		if((self. __modelType != (8 + 16)) or not (self. __exportAnimation) or not (((self. __objSelectFlags)[objIndex]))):
			return
		
		self. __curObjShortName = (((self. __objShortNames)[objIndex]))
		
		self. __WriteLine("\t*TM_ANIMATION {")
		self. __WriteLine("\t\t*NODE_NAME \"" + self. __curObjShortName + "\"")
		self. __WriteLine("\t\t*CONTROL_POS_TRACK {")
		caption = (FormatMsg1("Writing '%1'", self. __curObjShortName))
		
		start = (self. __timeTransform.GetStartFrameInScene())
		end = (self. __timeTransform.GetEndFrameInScene())
		for frameInScene in range((start), (end + 1)):
			show_progress_bar(caption, 60 + 20 * (frameInScene - start) / (end - start + 1))
			frameInFile = (self. __TransformTime(frameInScene))
			str = ("\t\t\t*CONTROL_POS_SAMPLE " + (int_to_string(frameInFile * (4800 / get_fps()))))
			pos = (((self. __posTracks)[objIndex]).GetSamplePos(frameInScene - start))
			pos = (pos * self. __scaleCoef)
			str = (str + "\t" + self. __Point3ToString(pos))
			self. __WriteLine(str)
		
		self. __WriteLine("\t\t}")
		self. __WriteLine("\t\t*CONTROL_ROT_TRACK {")
		for frameInScene in range((start), (end + 1)):
			show_progress_bar(caption, 80 + 20 * (frameInScene - start) / (end - start + 1))
			frameInFile = (self. __TransformTime(frameInScene))
			str = ("\t\t\t*CONTROL_ROT_SAMPLE " + (int_to_string(frameInFile * (4800 / get_fps()))))
			axis = (((self. __rotTracks)[objIndex]).GetSampleAxis(frameInScene - start))
			angle = (((self. __rotTracks)[objIndex]).GetSampleAngle(frameInScene - start))
			str = (str + "\t" + self. __Point3ToString(axis))
			str = (str + "\t" + (float_to_string(angle)))
			self. __WriteLine(str)
		
		self. __WriteLine("\t\t}")
		self. __WriteLine("\t\t*CONTROL_SCALE_TRACK {")
		self. __WriteLine("\t\t\t*CONTROL_SCALE_SAMPLE 0 1 1 1 0 0 0 0")
		self. __WriteLine("\t\t}")
		self. __WriteLine("\t}")

	def __WriteGeomObject(self, objIndex):
		self. __curObjShortName = (((self. __objShortNames)[objIndex]))
		self. __curObjName = (self. __modelPrefix + self. __curObjShortName)
		caption = (FormatMsg1("Writing '%1'", self. __curObjShortName))
		show_progress_bar(caption, 1)
		self. __curObj = ((find_object_by_name(self. __curObjName)))
		if(self. __curObj == None):
			return

		objType = (((self. __objTypes)[objIndex]))
		if(objType == 3):
			self. __WriteLine("*GEOMOBJECT {")
		else:
			self. __WriteLine("*HELPEROBJECT {")

		self. __WriteLine("\t*NODE_NAME \"" + self. __curObjShortName + "\"")
		
		parentName = (((self. __parentShortNames)[objIndex]))
		if(parentName != ""):
			self. __WriteLine("\t*NODE_PARENT \"" + parentName + "\"")

		tm = ((get_transform(self. __curObj)))
		if(self. __modelType == (1 + 2)):
			tm = (identity_matrix())
		
		self. __WriteNodeTM(tm)
		
		if((objType == 3) and (is_mesh_object(self. __curObj))):
			self. __WriteMaterialRef(0)
			if((self. __modelType == (1 + 2)) and (self. __exportAnimation)):
				self. __WriteLine("\t*MESH_ANIMATION {")
				oldCurFrame = (get_current_frame())
				
				start = (self. __timeTransform.GetStartFrameInScene())
				end = (self. __timeTransform.GetEndFrameInScene())
				for frameInScene in range((start), (end + 1)):
					set_current_frame(frameInScene)
					self. __WriteMesh()
				
				set_current_frame(oldCurFrame)
				self. __WriteLine("\t}")
			else:
				self. __WriteMesh()

		self. __WriteTMAnimation(objIndex)
		self. __WriteLine("}")

	def __WriteGeomObjects(self):
		for i in range((0), ((len(self. __objShortNames)))):
			objType = (((self. __objTypes)[i]))
			if(((self. __objSelectFlags)[i]) or (self. __exportAnimation and (objType == 1 or objType == 2))):
				self. __WriteGeomObject(i)

	def __WriteSkinWeights(self):
		caption = ("Writing skin weights")
		show_progress_bar(caption, 99)
		
		for skinnedObjIndex in range((0), ((len(self. __skinnedObjs)))):
			self. __curObj = (((self. __skinnedObjs)[skinnedObjIndex]))
			self. __curObjName = ((get_object_name(self. __curObj)))
			na = (AnalyzeName(self. __curObjName))
			self. __curObjShortName = (na.GetShortName())
			sd = (SkinData(self. __curObj))
			numVerts = ((sd.get_num_verts()))
			
			self. __WriteLine("*MESH_SOFTSKINVERTS {")
			self. __WriteLine(self. __curObjShortName)
			self. __WriteLine((int_to_string(numVerts)))
			for i in range((0), (numVerts)):
				numWeights = ((sd.get_vert_num_weights(i)))
				str = ((int_to_string(numWeights)))
				for j in range((0), (numWeights)):
					weight = ((sd.get_vert_weight(i, j)))
					bone = ((sd.get_vert_weight_bone(i, j)))
					boneName = ((get_object_name(bone)))
					na.Analyze(boneName)
					boneShortName = (na.GetShortName())
					str = (str + "\t\"" + boneShortName + "\"\t" + (float_to_string(weight)))
				
				self. __WriteLine(str)
			
			self. __WriteLine("}")

	def __RetrieveTMAnimationFromScene(self):
		if((self. __modelType != (8 + 16)) or not (self. __exportAnimation)):
			return

		numSamples = (self. __timeTransform.GetEndFrameInFile() - self. __timeTransform.GetStartFrameInFile() + 1)
		for i in range((0), ((len(self. __objShortNames)))):
			posTrack = (NewPosTrack())
			rotTrack = (NewRotTrack())
			if(((self. __objSelectFlags)[i])):
				posTrack.SetNumSamples(numSamples)
				rotTrack.SetNumSamples(numSamples)
			
			self. __posTracks.append(posTrack)
			self. __rotTracks.append(rotTrack)

		oldCurFrame = (get_current_frame())
		start = (self. __timeTransform.GetStartFrameInScene())
		end = (self. __timeTransform.GetEndFrameInScene())
		for frameInScene in range((start), (end + 1)):
			set_current_frame(frameInScene)
			frameOffset = (frameInScene - start)
			for i in range((0), ((len(self. __objShortNames)))):
				if(((self. __objSelectFlags)[i])):
					self. __curObjShortName = (((self. __objShortNames)[i]))
					self. __curObjName = (self. __modelPrefix + self. __curObjShortName)
					self. __curObj = ((find_object_by_name(self. __curObjName)))
					if(self. __curObj != None):
						tm = ((get_transform(self. __curObj)))
						pos = (get_translation_part(tm))
						q = (get_rotation_part(tm))
						axis = (get_axis(q))
						angle = (get_angle(q))
						((self. __posTracks)[i]).SetSamplePos(frameOffset, pos)
						((self. __rotTracks)[i]).SetSampleAxis(frameOffset, axis)
						((self. __rotTracks)[i]).SetSampleAngle(frameOffset, angle)

		set_current_frame(oldCurFrame)

	def Init(self):
		self. __file = (NewFile())
		self. __modelHierarchy = (NewModelHierarchy())
		self. __modelPrefix = ("")
		self. __modelType = (0)
		self. __objShortNames = ([])
		self. __parentShortNames = ([])
		self. __objTypes = ([])
		self. __objSelectFlags = ([])
		self. __exportAnimation = (False)
		self. __scaleCoef = (1)
		self. __timeTransform = (NewTimeTransform())
		self. __expMtls = ([])
		self. __addMtls = ([])
		self. __curObj = (None)
		self. __curObjName = ("")
		self. __curObjShortName = ("")
		self. __skinnedObjs = ([])
		self. __posTracks = ([])
		self. __rotTracks = ([])	

	def WriteASCFile(self, filename, modelHierarchy, selObjShortNames, exportAnimation, timeTransform, spaceTransform):
		self. Init()
		self. __modelHierarchy = (modelHierarchy)
		self. __modelPrefix = (modelHierarchy.GetModelPrefix())
		self. __modelType = (modelHierarchy.GetModelType())
		self. __objShortNames = (modelHierarchy.GetObjects())
		self. __parentShortNames = (modelHierarchy.GetObjectParents())
		self. __objTypes = (modelHierarchy.GetObjectTypes())
		
		self. __objSelectFlags = ([])
		for i in range((0), ((len(self. __objShortNames)))):
			selFlag = (False)
			for j in range((0), ((len(selObjShortNames)))):
				if(((self. __objShortNames)[i]) == ((selObjShortNames)[j])):
					selFlag = (True)
					break

			self. __objSelectFlags.append(selFlag)

		self. __exportAnimation = (exportAnimation)
		self. __scaleCoef = (spaceTransform.GetFileUnitsPerSystemUnit())
		self. __timeTransform = (timeTransform)
		try:
			self. __file.Open(filename, "wt")
			self. __WriteASCVersion()
			self. __WriteComment()
			self. __WriteSceneInfo()
			self. __WriteMaterialList()
			self. __RetrieveTMAnimationFromScene()
			self. __WriteGeomObjects()
			self. __WriteSkinWeights()
			self. __file.Close()
		
		except RuntimeError as ex:
			self. __file.Close()
			delete_file(filename)
			raise

def NewASCFileSaver():
	saver = (TASCFileSaver())
	saver.Init()
	return (saver)

class TASCExporterDlgInput:
	def Init(self):
		self. __exportFileName = ("")
		self. __modelHierarchies = ([])
		self. __selectedPrefix = ("")
		self. __selectedObjects = ([])
		self. __startFrame = (0)
		self. __endFrame = (100)
		self. __fileUnitsPerSystemUnit = (1)	

	def Write(self, f):
		f.WriteString(self. __exportFileName)
		numModels = ((len(self. __modelHierarchies)))
		f.WriteUnsignedLong(numModels)
		for i in range((0), (numModels)):
			((self. __modelHierarchies)[i]).Write(f)
		
		f.WriteString(self. __selectedPrefix)
		numSelectedObjects = ((len(self. __selectedObjects)))
		f.WriteUnsignedLong(numSelectedObjects)
		for i in range((0), (numSelectedObjects)):
			f.WriteString(((self. __selectedObjects)[i]))
		
		f.WriteSignedLong(self. __startFrame)
		f.WriteSignedLong(self. __endFrame)
		f.WriteFloat(self. __fileUnitsPerSystemUnit)

	def Read(self, f):
		self. __exportFileName = (f.ReadString())
		numModels = (f.ReadUnsignedLong())
		self. __modelHierarchies = ([])
		for i in range((0), (numModels)):
			mh = (NewModelHierarchy())
			mh.Read(f)
			self. __modelHierarchies.append(mh)
		
		self. __selectedPrefix = (f.ReadString())
		numSelectedObjects = (f.ReadUnsignedLong())
		self. __selectedObjects = ([])
		for i in range((0), (numSelectedObjects)):
			self. __selectedObjects.append(f.ReadString())
		
		self. __startFrame = (f.ReadSignedLong())
		self. __endFrame = (f.ReadSignedLong())
		self. __fileUnitsPerSystemUnit = (f.ReadFloat())

	def SetExportFileName(self, exportFileName):
		self. __exportFileName = (exportFileName)

	def GetExportFileName(self):
		return (self. __exportFileName)

	def SetModelHierarchies(self, modelHierarchies):
		self. __modelHierarchies = (modelHierarchies)

	def GetModelHierarchies(self):
		return (self. __modelHierarchies)

	def SetSelectedPrefix(self, selectedPrefix):
		self. __selectedPrefix = (selectedPrefix)

	def GetSelectedPrefix(self):
		return (self. __selectedPrefix)

	def SetSelectedObjects(self, selectedObjects):
		self. __selectedObjects = (selectedObjects)

	def GetSelectedObjects(self):
		return (self. __selectedObjects)

	def SetStartFrame(self, frameIndex):
		self. __startFrame = (frameIndex)

	def GetStartFrame(self):
		return (self. __startFrame)

	def SetEndFrame(self, frameIndex):
		self. __endFrame = (frameIndex)

	def GetEndFrame(self):
		return (self. __endFrame)

	def SetFileUnitsPerSystemUnit(self, scaleCoef):
		self. __fileUnitsPerSystemUnit = (scaleCoef)

	def GetFileUnitsPerSystemUnit(self):
		return (self. __fileUnitsPerSystemUnit)

def NewASCExporterDlgInput():
	dlgInput = (TASCExporterDlgInput())
	dlgInput.Init()
	return (dlgInput)

class TASCExporterDlgOutput:
	def Init(self, dlgInput):
		self. __selectedPrefix = (dlgInput.GetSelectedPrefix())
		self. __selectedObjects = (dlgInput.GetSelectedObjects())
		self. __exportAnimation = (False)
		self. __continueExport = (True)
		
		self. __spaceTransform = (NewSpaceTransform())
		self. __spaceTransform.SetFileUnitsPerSystemUnit(dlgInput.GetFileUnitsPerSystemUnit())
		
		self. __timeTransform = (NewTimeTransform())
		self. __timeTransform.SetMinFrameInScene(dlgInput.GetStartFrame())
		self. __timeTransform.SetMaxFrameInScene(dlgInput.GetEndFrame())
		self. __timeTransform.SetMinFrameInFile( -32768)
		self. __timeTransform.SetMaxFrameInFile(32767)
		self. __timeTransform.SetStartFrameInScene(dlgInput.GetStartFrame())
		self. __timeTransform.SetEndFrameInScene(dlgInput.GetEndFrame())
		frameOffset = (0)
		if(dlgInput.GetStartFrame() > 900):
			frameOffset = ( -1000)
		
		self. __timeTransform.SetStartFrameInFile(dlgInput.GetStartFrame() + frameOffset)
		self. __timeTransform.SetEndFrameInFile(dlgInput.GetEndFrame() + frameOffset)	

	def Write(self, f):
		f.WriteString(self. __selectedPrefix)
		numSelectedObjects = ((len(self. __selectedObjects)))
		f.WriteUnsignedLong(numSelectedObjects)
		for i in range((0), (numSelectedObjects)):
			f.WriteString(((self. __selectedObjects)[i]))
		
		f.WriteBool(self. __exportAnimation)
		self. __spaceTransform.Write(f)
		self. __timeTransform.Write(f)
		f.WriteBool(self. __continueExport)

	def Read(self, f):
		self. __selectedPrefix = (f.ReadString())
		numSelectedObjects = (f.ReadUnsignedLong())
		self. __selectedObjects = ([])
		for i in range((0), (numSelectedObjects)):
			self. __selectedObjects.append(f.ReadString())
		
		self. __exportAnimation = (f.ReadBool())
		self. __spaceTransform.Read(f)
		self. __timeTransform.Read(f)
		self. __continueExport = (f.ReadBool())

	def SetContinueExport(self, continueExport):
		self. __continueExport = (continueExport)

	def GetContinueExport(self):
		return (self. __continueExport)

	def SetSelectedPrefix(self, selectedPrefix):
		self. __selectedPrefix = (selectedPrefix)

	def GetSelectedPrefix(self):
		return (self. __selectedPrefix)

	def SetSelectedObjects(self, selectedObjects):
		self. __selectedObjects = (selectedObjects)

	def GetSelectedObjects(self):
		return (self. __selectedObjects)

	def SetExportAnimation(self, exportAnimation):
		self. __exportAnimation = (exportAnimation)

	def GetExportAnimation(self):
		return (self. __exportAnimation)

	def SetTimeTransform(self, timeTransform):
		self. __timeTransform = (timeTransform)

	def GetTimeTransform(self):
		return (self. __timeTransform)

	def SetSpaceTransform(self, spaceTransform):
		self. __spaceTransform = (spaceTransform)

	def GetSpaceTransform(self):
		return (self. __spaceTransform)

def NewASCExporterDlgOutput(dlgInput):
	dlgOutput = (TASCExporterDlgOutput())
	dlgOutput.Init(dlgInput)
	return (dlgOutput)

def RunUIExe():
	cmdline = ("")
	cmdline = (cmdline + "\"" + (get_root_dir() + "KrxImpExp\\wxImpExpUI.exe") + "\"")
	cmdline = (cmdline + " ")
	cmdline = (cmdline + "\"" + (get_plugcfg_dir() + "KrxImpExpDlgInput.bin") + "\"")
	cmdline = (cmdline + " ")
	cmdline = (cmdline + "\"" + (get_plugcfg_dir() + "KrxImpExpDlgOutput.bin") + "\"")
	cmdline = (cmdline + " ")
	cmdline = (cmdline + "\"" + (get_plugcfg_dir() + "KrxImpExpUI.cfg") + "\"")
	errCode = (system("\"" + cmdline + "\""))
	if(errCode != 0):
		raise RuntimeError(FormatMsg1("Could not execute:\n%1", (get_root_dir() + "KrxImpExp\\wxImpExpUI.exe")))

def register(): 
	register_exporter("KrxAscExp", "ASC", "Kerrax ASCII Model") 

def unregister(): 
	unregister_exporter("KrxAscExp") 

if __name__ == "main": 
	register() 

def KrxAscExp(filename_param, quiet_param = False): 
	QUIET = quiet_param 
	EXPORT_FILE_NAME = filename_param
	
	sceneAnalyzer = (AnalyzeScene())
	
	dlgInput = (NewASCExporterDlgInput())
	dlgInput.SetExportFileName(EXPORT_FILE_NAME)
	dlgInput.SetModelHierarchies(sceneAnalyzer.GetModelHierarchies())
	selectedPrefixes = (sceneAnalyzer.GetSelectedPrefixes())
	if((len(selectedPrefixes)) == 0):
		selectedPrefixes = (sceneAnalyzer.GetScenePrefixes())
	
	if((len(selectedPrefixes)) != 0):
		prefix = (((selectedPrefixes)[0]))
		dlgInput.SetSelectedPrefix(prefix)
		dlgInput.SetSelectedObjects(sceneAnalyzer.GetModelHierarchyByPrefix(prefix).GetObjects())
	
	dlgInput.SetStartFrame(get_start_frame())
	dlgInput.SetEndFrame(get_end_frame())
	dlgInput.SetFileUnitsPerSystemUnit(100)
	
	dlgOutput = (NewASCExporterDlgOutput(dlgInput))
	inputFile = (NewFile())
	outputFile = (NewFile())
	if( not (QUIET)):
		try:
			show_progress_bar("Showing dialog", 0)
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("ASCExporterDlgInput")
			dlgInput.Write(inputFile)
			inputFile.Close()
			RunUIExe()
			outputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgOutput.bin"), "rb")
			structName = (outputFile.ReadString())
			dlgOutput.Read(outputFile)
			outputFile.Close()
			
			if( not (dlgOutput.GetContinueExport())):
				hide_progress_bar()
				return (2)

		except RuntimeError as ex:
			inputFile.Close()
			outputFile.Close()
			show_error_box("Kerrax ASC Exporter", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
			hide_progress_bar()
			return (2)

	saver = (NewASCFileSaver())
	try:
		selPrefix = (dlgOutput.GetSelectedPrefix())
		modelHierarchy = (sceneAnalyzer.GetModelHierarchyByPrefix(selPrefix))
		selObjects = (dlgOutput.GetSelectedObjects())
		expAnim = (dlgOutput.GetExportAnimation())
		timeTransform = (dlgOutput.GetTimeTransform())
		spaceTransform = (dlgOutput.GetSpaceTransform())
		saver.WriteASCFile(EXPORT_FILE_NAME, modelHierarchy, selObjects, expAnim, timeTransform, spaceTransform)
	
	except RuntimeError as ex:
		show_error_box("Kerrax ASC Exporter", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
		hide_progress_bar()
		return (0)

	hide_progress_bar()
	return (1)


