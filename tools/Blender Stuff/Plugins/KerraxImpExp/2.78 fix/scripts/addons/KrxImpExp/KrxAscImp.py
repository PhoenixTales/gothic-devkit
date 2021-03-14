
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

class TObjectDisplayProps:
	def Init(self):
		self. __visibility = (True)
		self. __renderable = (True)
		self. __transparent = (False)
		self. __boxMode = (False)

	def Reset(self):
		self. Init()

	def GetVisibility(self):
		return (self. __visibility)

	def SetVisibility(self, visibility):
		self. __visibility = (visibility)

	def GetRenderable(self):
		return (self. __renderable)

	def SetRenderable(self, renderable):
		self. __renderable = (renderable)

	def GetTransparent(self):
		return (self. __transparent)

	def SetTransparent(self, transparent):
		self. __transparent = (transparent)

	def GetBoxMode(self):
		return (self. __boxMode)

	def SetBoxMode(self, boxMode):
		self. __boxMode = (boxMode)

def NewObjectDisplayProps():
	props = (TObjectDisplayProps())
	props.Init()
	return (props)

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

def DelObjects(objects):
	i = ((len(objects)) - 1)
	while(i >= 0):
		obj = (((objects)[i]))
		delete_object(obj)
		i = (i - 1)

class TASCFileLoader:
	def __IsFDigit(self, ch):
		if((ch == "0") or (ch == "1") or (ch == "2") or (ch == "3")):
			return (True)
		
		if((ch == "4") or (ch == "5") or (ch == "6") or (ch == "7")):
			return (True)
		
		if((ch == "8") or (ch == "9") or (ch == "+") or (ch == "-")):
			return (True)
		
		if((ch == ".") or (ch == "e") or (ch == "E")):
			return (True)
		
		return (False)

	def __ReadTokenSafe(self):
		ch = (" ")
		while((ch == " ") or (ch == "\t")):
			if(((self. __column) > len(self. __lineText)) or (self. __line == 0)):
				if(self. __file.Eof()):
					return (self. __tokenEOF)
				
				self. __line = (self. __line + 1)
				self. __lineText = (self. __file.ReadLine())
				self. __column = (1)

			ch = ((self. __lineText)[self. __column - 1:self. __column])
			self. __column = (self. __column + 1)

		self. __tokenLine = (self. __line)
		self. __tokenColumn = (self. __column - 1)
		
		self. __token3 = (self. __token2)
		self. __token2 = (self. __token1)
		self. __token1 = (self. __token)
		self. __token = (ch) 
		
		if(ch == "\""):
			while(self. __column <= len(self. __lineText)):
				ch = ((self. __lineText)[self. __column - 1:self. __column])
				self. __column = (self. __column + 1)
				self. __token = (self. __token + ch)
				if(ch == "\""):
					break

		elif(self. __IsFDigit(ch)):
			while(self. __column <= len(self. __lineText)):
				ch = ((self. __lineText)[self. __column - 1:self. __column])
				if( not (self. __IsFDigit(ch))):
					break
				
				self. __column = (self. __column + 1)
				self. __token = (self. __token + ch)
			
		else:
			while(self. __column <= len(self. __lineText)):
				ch = ((self. __lineText)[self. __column - 1:self. __column])
				if(ch == " " or ch == "\t"):
					break
				
				self. __column = (self. __column + 1)
				self. __token = (self. __token + ch)

		return (self. __token)

	def __ReadLine(self):
		if(((self. __column) > len(self. __lineText)) or (self. __line == 0)):
			if(self. __file.Eof()):
				raise RuntimeError(FormatMsg1("Unexpected end of file.\nFile name: \"%1\".", self. __filename))
			
			self. __line = (self. __line + 1)
			self. __lineText = (self. __file.ReadLine())
			self. __column = (1)

		self. __tokenLine = (self. __line)
		self. __tokenColumn = (self. __column)
		
		self. __token3 = (self. __token2)
		self. __token2 = (self. __token1)
		self. __token1 = (self. __token)
		
		self. __token = ((self. __lineText)[self. __column - 1:len(self. __lineText)])
		self. __column = (len(self. __lineText) + 1)
		
		return (self. __token)

	def __ReadToken(self):
		token = (self. __ReadTokenSafe())
		if(token == ""):
			raise RuntimeError(FormatMsg1("Unexpected end of file.\nFile name: \"%1\".", self. __filename))
		
		return (token)

	def __ReadCertainToken(self, tokenRequired):
		tokenRead = (self. __ReadToken())
		if(stricmp(tokenRequired, tokenRead) != 0):
			raise RuntimeError(FormatMsg5("Unexpected token: \"%1\".\nToken required here: \"%2\".\nLine: %3, column: %4.\nFile name: \"%5\".", tokenRead, tokenRequired, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))

	def __ReadString(self):
		token = (self. __ReadToken())
		toklen = (len(token))
		firstChar = ((token)[0:1])
		lastChar = ((token)[toklen - 1:toklen])
		if(toklen < 2 or firstChar != "\"" or lastChar != "\""):
			raise RuntimeError(FormatMsg4("Unexpected token: \"%1\".\nRequired a quoted string here.\nLine: %2, column: %3.\nFile name: \"%4\".", token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
		
		return ((token)[1:toklen - 1])

	def __ReadInt(self):
		token = (self. __ReadToken())
		r = ((string_to_int(token)))
		if(r == None):
			raise RuntimeError(FormatMsg4("Unexpected token: \"%1\".\nRequired an integer number here.\nLine: %2, column: %3.\nFile name: \"%4\".", token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
		
		return (r)

	def __ReadFloat(self):
		token = (self. __ReadToken())
		r = ((string_to_float(token)))
		if(r == None):
			raise RuntimeError(FormatMsg4("Unexpected token: \"%1\".\nRequired a floating point number here.\nLine: %2, column: %3.\nFile name: \"%4\".", token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
		
		return (r)

	def __ReadPoint3(self):
		x = (self. __ReadFloat())
		y = (self. __ReadFloat())
		z = (self. __ReadFloat())
		return (new_vector(x, y, z))

	def __ReadUVVert(self):
		u = (self. __ReadFloat())
		v = (self. __ReadFloat())
		w = (self. __ReadFloat())
		return (new_uvvert(u, v))

	def __ReadColor(self):
		r = (self. __ReadFloat())
		g = (self. __ReadFloat())
		b = (self. __ReadFloat())
		return (new_color(r, g, b))

	def __ReadASCVersion(self):
		ver = (self. __ReadInt())

	def __ReadComment(self):
		comment = (self. __ReadString())

	def __ReadSceneInfo(self):
		caption = ("Reading scene info")
		show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
		level = (0)
		token = ("")
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*SCENE_FIRSTFRAME") == 0):
				self. __animStartFrame = (self. __ReadInt())
			elif(stricmp(token, "*SCENE_LASTFRAME") == 0):
				self. __animEndFrame = (self. __ReadInt())
			elif(stricmp(token, "*SCENE_FRAMESPEED") == 0):
				self. __animFrameRate = (self. __ReadInt())
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadMap(self):
		mapFileName = ("")
		level = (0)
		token = ("")
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*BITMAP") == 0):
				mapFileName = (self. __ReadString())
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

		return (mapFileName)

	def __ReadSubMaterial(self):
		mtlName = ("")
		mtlClass = ("")
		clrDiffuse = (black_color())
		mapDiffuse = ("")
		level = (0)
		token = ("")
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*MATERIAL_NAME") == 0):
				mtlName = (self. __ReadString())
			elif(stricmp(token, "*MATERIAL_CLASS") == 0):
				mtlClass = (self. __ReadString())
				if(stricmp(mtlClass, "Standard") != 0):
					raise RuntimeError(FormatMsg4("Invalid class of sub-material: %1.\nSupported only the \"Standard\" sub-material class.\nLine: %2, column: %3.\nFile name: \"%4\".", token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
			elif(stricmp(token, "*MATERIAL_DIFFUSE") == 0):
				clrDiffuse = (self. __ReadColor())
			elif(stricmp(token, "*MAP_DIFFUSE") == 0):
				mapDiffuse = (self. __ReadMap())
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

		mtlDesc = (NewMaterialDesc())
		mtlDesc.SetMaterialName(mtlName)
		mtlDesc.SetDiffuseMapFilename(mapDiffuse)
		mtlDesc.SetDiffuseColor(clrDiffuse)
		return (mtlDesc)

	def __ReadMaterial(self):
		mtlName = ("")
		mtlClass = ("")
		clrDiffuse = (black_color())
		mapDiffuse = ("")
		numSubMtls = (0)
		subMtls = ([])
		subMtlIndex = (0)
		level = (0)
		token = ("")
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*MATERIAL_NAME") == 0):
				mtlName = (self. __ReadString())
				caption = ("Reading materials")
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			elif(stricmp(token, "*MATERIAL_CLASS") == 0):
				mtlClass = (self. __ReadString())
				if(stricmp(mtlClass, "Standard") != 0 and stricmp(mtlClass, "Multi/Sub-Object") != 0):
					raise RuntimeError(FormatMsg4("Invalid class of material: %1.\nSupported only the \"Standard\" and \"Multi/Sub-Object\" material classes.\nLine: %2, column: %3.\nFile name: \"%4\".", token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
			elif(stricmp(token, "*MATERIAL_DIFFUSE") == 0):
				clrDiffuse = (self. __ReadColor())
			elif(stricmp(token, "*MAP_DIFFUSE") == 0):
				mapDiffuse = (self. __ReadMap())
			elif(stricmp(token, "*NUMSUBMTLS") == 0):
				numSubMtls = (self. __ReadInt())
				subMtls = ([])
				for i in range((0), (numSubMtls)):
					subMtls.append(NewMaterialDesc())
				
			elif(stricmp(token, "*SUBMATERIAL") == 0):
				subMtlIndex = (self. __ReadInt())
				if(subMtlIndex < 0 or subMtlIndex >= numSubMtls):
					raise RuntimeError(FormatMsg6("Sub-material index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numSubMtls - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				subMtls[subMtlIndex] = (self. __ReadSubMaterial())
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

		if(numSubMtls == 0):
			mtlDesc = (NewMaterialDesc())
			mtlDesc.SetMaterialName(mtlName)
			mtlDesc.SetDiffuseMapFilename(mapDiffuse)
			mtlDesc.SetDiffuseColor(clrDiffuse) 
			subMtls.append(mtlDesc)
		
		return (subMtls)

	def __ReadMaterialList(self):
		self. __materialsDesc = ([])
		level = (0)
		token = ("")
		numMtls = (0)
		mtlIndex = (0)
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*MATERIAL_COUNT") == 0):
				numMtls = (self. __ReadInt())
				self. __materialsDesc = ([])
				for i in range((0), (numMtls)):
					self. __materialsDesc.append([])
				
			elif(stricmp(token, "*MATERIAL") == 0):
				mtlIndex = (self. __ReadInt())
				if(mtlIndex < 0 or mtlIndex >= numMtls):
					raise RuntimeError(FormatMsg6("Material index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numMtls - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				self. __materialsDesc[mtlIndex] = (self. __ReadMaterial())
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadMaterialRef(self, objDesc):
		mtlIndex = (self. __ReadInt())
		objDesc.SetMaterialRef(mtlIndex)

	def __ReadNodeTM(self, objDesc):
		level = (0)
		token = ("")
		tm = (identity_matrix())
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*TM_ROW0") == 0):
				set_row0(tm, self. __ReadPoint3())
			elif(stricmp(token, "*TM_ROW1") == 0):
				set_row1(tm, self. __ReadPoint3())
			elif(stricmp(token, "*TM_ROW2") == 0):
				set_row2(tm, self. __ReadPoint3())
			elif(stricmp(token, "*TM_ROW3") == 0):
				set_row3(tm, self. __ReadPoint3())
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

		objDesc.SetTransform(tm)

	def __ReadVertexList(self, objDesc):
		level = (0)
		token = ("")
		vertIndex = (0)
		pt = (zero_vector())
		matWorldToLocal = (inverse_matrix(objDesc.GetTransform()))
		mshDesc = (objDesc.GetMeshDesc())
		numVerts = (mshDesc.GetNumVerts())
		while(True):
			if(((vertIndex) & (15)) == 0):
				caption = (FormatMsg1("Reading '%1'", objDesc.GetObjectName()))
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			token = (self. __ReadToken())
			if(stricmp(token, "*MESH_VERTEX") == 0):
				vertIndex = (self. __ReadInt())
				if(vertIndex < 0 or vertIndex >= numVerts):
					raise RuntimeError(FormatMsg6("Vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				pt = (self. __ReadPoint3())
				pt = (multiply_vector_matrix(pt, matWorldToLocal))
				mshDesc.SetVert(vertIndex, pt)
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadFaceList(self, objDesc):
		level = (0)
		token = ("")
		faceIndex = (0)
		mshDesc = (objDesc.GetMeshDesc())
		numFaces = (mshDesc.GetNumFaces())
		numVerts = (mshDesc.GetNumVerts())
		while(True):
			if(((faceIndex) & (15)) == 0):
				caption = (FormatMsg1("Reading '%1'", objDesc.GetObjectName()))
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			token = (self. __ReadToken())
			if(stricmp(token, "*MESH_FACE") == 0):
				faceIndex = (self. __ReadInt())
				if(faceIndex < 0 or faceIndex >= numFaces):
					raise RuntimeError(FormatMsg6("Face index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numFaces - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
			elif(stricmp(token, "A:") == 0):
				vA = (self. __ReadInt())
				if(vA < 0 or vA >= numVerts):
					raise RuntimeError(FormatMsg6("Vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				f = (mshDesc.GetFace(faceIndex))
				set_face_vert(f, 0, vA)
				mshDesc.SetFace(faceIndex, f)
			elif(stricmp(token, "B:") == 0):
				vB = (self. __ReadInt())
				if(vB < 0 or vB >= numVerts):
					raise RuntimeError(FormatMsg6("Vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				f = (mshDesc.GetFace(faceIndex))
				set_face_vert(f, 1, vB)
				mshDesc.SetFace(faceIndex, f)
			elif(stricmp(token, "C:") == 0):
				vC = (self. __ReadInt())
				if(vC < 0 or vC >= numVerts):
					raise RuntimeError(FormatMsg6("Vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				f = (mshDesc.GetFace(faceIndex))
				set_face_vert(f, 2, vC)
				mshDesc.SetFace(faceIndex, f)
			elif(stricmp(token, "AB:") == 0):
				edgeAB = (True)
				if(self. __ReadInt() == 0):
					edgeAB = (False)
				
				mshDesc.SetEdgeVis(faceIndex, 0, edgeAB)
			elif(stricmp(token, "BC:") == 0):
				edgeBC = (True)
				if(self. __ReadInt() == 0):
					edgeBC = (False)
				
				mshDesc.SetEdgeVis(faceIndex, 1, edgeBC)
			elif(stricmp(token, "CA:") == 0):
				edgeCA = (True)
				if(self. __ReadInt() == 0):
					edgeCA = (False)
				
				mshDesc.SetEdgeVis(faceIndex, 2, edgeCA)
			elif(stricmp(token, "*MESH_SMOOTHING") == 0):
				token = (self. __ReadToken())
				smGroup = ((string_to_int(token)))
				if(smGroup != None):
					mshDesc.SetFaceSmoothGroup(faceIndex, smGroup)

			if(stricmp(token, "*MESH_MTLID") == 0):
				faceMtlIndex = (self. __ReadInt())
				mshDesc.SetFaceMatID(faceIndex, faceMtlIndex)
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadTVertList(self, obj):
		level = (0)
		token = ("")
		tVertIndex = (0)
		mshDesc = (obj.GetMeshDesc())
		numTVerts = (mshDesc.GetNumTVerts())
		while(True):
			if(((tVertIndex) & (15)) == 0):
				caption = (FormatMsg1("Reading '%1'", obj.GetObjectName()))
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			token = (self. __ReadToken())
			if(stricmp(token, "*MESH_TVERT") == 0):
				tVertIndex = (self. __ReadInt())
				if(tVertIndex < 0 or tVertIndex >= numTVerts):
					raise RuntimeError(FormatMsg6("Texture vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numTVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				uv = (self. __ReadUVVert())
				mshDesc.SetTVert(tVertIndex, uv)
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadTFaceList(self, objDesc):
		level = (0)
		token = ("")
		tFaceIndex = (0)
		mshDesc = (objDesc.GetMeshDesc())
		numFaces = (mshDesc.GetNumFaces())
		numTVerts = (mshDesc.GetNumTVerts())
		while(True):
			if(((tFaceIndex) & (15)) == 0):
				caption = (FormatMsg1("Reading '%1'", objDesc.GetObjectName()))
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			token = (self. __ReadToken())
			if(stricmp(token, "*MESH_TFACE") == 0):
				tFaceIndex = (self. __ReadInt())
				if(tFaceIndex < 0 or tFaceIndex >= numFaces):
					raise RuntimeError(FormatMsg6("Face index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numFaces - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				tA = (self. __ReadInt())
				if(tA < 0 or tA >= numTVerts):
					raise RuntimeError(FormatMsg6("Texture vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numTVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				tB = (self. __ReadInt())
				if(tB < 0 or tB >= numTVerts):
					raise RuntimeError(FormatMsg6("Texture vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numTVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				tC = (self. __ReadInt())
				if(tC < 0 or tC >= numTVerts):
					raise RuntimeError(FormatMsg6("Texture vertex index %1 is out of range.\nAllowable range: %2..%3.\nLine: %4, column: %5.\nFile name: \"%6\".", self. __token, "0", (int_to_string(numTVerts - 1)), (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				mshDesc.SetTVFace(tFaceIndex, new_tvface(tA, tB, tC))
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadMesh(self, objDesc):
		level = (0)
		token = ("")
		timeValue = (0)
		mshDesc = (objDesc.GetMeshDesc())
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*TIMEVALUE") == 0):
				timeValue = (self. __ReadInt())
			elif(stricmp(token, "*MESH_NUMVERTEX") == 0):
				mshDesc.SetNumVerts(self. __ReadInt())
			elif(stricmp(token, "*MESH_NUMFACES") == 0):
				mshDesc.SetNumFaces(self. __ReadInt())
			elif(stricmp(token, "*MESH_VERTEX_LIST") == 0):
				self. __ReadVertexList(objDesc)
			elif(stricmp(token, "*MESH_FACE_LIST") == 0):
				self. __ReadFaceList(objDesc)
			elif(stricmp(token, "*MESH_NUMTVERTEX") == 0):
				mshDesc.SetNumTVerts(self. __ReadInt())
			elif(stricmp(token, "*MESH_TVERTLIST") == 0):
				self. __ReadTVertList(objDesc)
			elif(stricmp(token, "*MESH_NUMTVFACES") == 0):
				mshDesc.SetNumTVFaces(self. __ReadInt())
			elif(stricmp(token, "*MESH_TFACELIST") == 0):
				self. __ReadTFaceList(objDesc)
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadTMAnimation(self, objDesc):
		self. __numTMAnimations = (self. __numTMAnimations + 1)
		
		level = (0)
		token = ("")
		posTrack = (objDesc.GetPosTrack())
		rotTrack = (objDesc.GetRotTrack())
		
		numSamples = (self. __animEndFrame - self. __animStartFrame + 1)
		posTrack.SetNumSamples(numSamples)
		rotTrack.SetNumSamples(numSamples)
		
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*NODE_NAME") == 0):
				caption = (FormatMsg1("Reading '%1'", objDesc.GetObjectName()))
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			elif(stricmp(token, "*CONTROL_POS_TRACK") == 0):
				self. __ReadCertainToken("{")
				posTrack.SetNumSamples(numSamples)
				for i in range((0), (numSamples)):
					self. __ReadCertainToken("*CONTROL_POS_SAMPLE")
					tick = (self. __ReadInt())
					posTrack.SetSamplePos(i, self. __ReadPoint3())
				
				self. __ReadCertainToken("}")
			elif(stricmp(token, "*CONTROL_ROT_TRACK") == 0):
				self. __ReadCertainToken("{")
				rotTrack.SetNumSamples(numSamples)
				for i in range((0), (numSamples)):
					self. __ReadCertainToken("*CONTROL_ROT_SAMPLE")
					tick = (self. __ReadInt())
					rotTrack.SetSampleAxis(i, self. __ReadPoint3())
					rotTrack.SetSampleAngle(i, self. __ReadFloat())
				
				self. __ReadCertainToken("}")
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

	def __ReadMeshAnimation(self, objDesc):
		self. __numMeshAnimations = (self. __numMeshAnimations + 1)
		
		level = (0)
		token = ("")
		morphTrack = (objDesc.GetMorphTrack())
		
		numSamples = (self. __animEndFrame - self. __animStartFrame + 1)
		morphTrack.SetNumSamples(numSamples)
		
		self. __ReadCertainToken("{")
		for i in range((0), (numSamples)):
			caption = (FormatMsg1("Reading '%1'", objDesc.GetObjectName()))
			show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			self. __ReadCertainToken("*MESH")
			numVerts = (0)
			while(True):
				token = (self. __ReadToken())
				if(stricmp(token, "*MESH_NUMVERTEX") == 0):
					numVerts = (self. __ReadInt())
				elif(stricmp(token, "*MESH_VERTEX_LIST") == 0):
					self. __ReadCertainToken("{")
					verts = ([])
					for j in range((0), (numVerts)):
						self. __ReadCertainToken("*MESH_VERTEX")
						vertIdx = (self. __ReadInt())
						pos = (self. __ReadPoint3())
						verts.append(pos)
					
					morphTrack.SetSampleVerts(i, verts)
					self. __ReadCertainToken("}")
				elif(token == "{"):
					level = (level + 1)
				elif(token == "}"):
					level = (level - 1)
				
				if(level == 0):
					break

		self. __ReadCertainToken("}")

	def __ReadGeomObject(self):
		objDesc = (NewObjectDesc())
		skipObj = (False)
		level = (0)
		token = ("")
		while(True):
			token = (self. __ReadToken())
			if(stricmp(token, "*NODE_NAME") == 0):
				objName = (self. __ReadString())
				objDesc.SetObjectName(objName)
				caption = (FormatMsg1("Reading '%1'", objName))
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
				for j in range((0), ((len(self. __objectsDesc)))):
					if(stricmp(((self. __objectsDesc)[j]).GetObjectName(), objName) == 0):
						raise RuntimeError(FormatMsg4("Cannot create an object with name \"%1\", because an object with the same name already exists.\nLine: %2, column: %3.\nFile name: \"%4\".", self. __token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))

				na = (AnalyzeName(objName))
				curObjType = (na.GetObjectType())
				objDesc.SetObjectType(curObjType)
				if(curObjType == 1):
					self. __numBones = (self. __numBones + 1)
					if(stricmp(objName, "Bip01") == 0):
						self. __bip01Found = (True)
					
				elif(curObjType == 2):
					self. __numSlots = (self. __numSlots + 1)
				elif(curObjType == 3):
					self. __numMeshes = (self. __numMeshes + 1)
				else:
					skipObj = (True)
				
			elif(stricmp(token, "*NODE_PARENT") == 0):
				parentName = (self. __ReadString())
				parentFound = (False)
				for j in range((0), ((len(self. __objectsDesc)))):
					if(stricmp(((self. __objectsDesc)[j]).GetObjectName(), parentName) == 0):
						parentFound = (True)
						break

				if( not (parentFound)):
					parentName = ("")
				
				objDesc.SetParentName(parentName)
			elif(stricmp(token, "*NODE_TM") == 0):
				self. __ReadNodeTM(objDesc)
			elif(stricmp(token, "*MATERIAL_REF") == 0):
				self. __ReadMaterialRef(objDesc)
			elif(stricmp(token, "*MESH") == 0):
				self. __ReadMesh(objDesc)
			elif(stricmp(token, "*MESH_SOFTSKIN") == 0):
				self. __ReadMesh(objDesc)
			elif(stricmp(token, "*TM_ANIMATION") == 0):
				self. __ReadTMAnimation(objDesc)
			elif(stricmp(token, "*MESH_ANIMATION") == 0):
				self. __ReadMeshAnimation(objDesc)
			elif(token == "{"):
				level = (level + 1)
			elif(token == "}"):
				level = (level - 1)
			
			if(level == 0):
				break

		if( not (skipObj)):
			self. __objectsDesc.append(objDesc)

	def __ReadSoftSkinVerts(self):
		self. __numSoftSkins = (self. __numSoftSkins + 1)
		self. __ReadCertainToken("{")
		objName = (self. __ReadLine())
		objIndex = ( -1)
		for j in range((0), ((len(self. __objectsDesc)))):
			if(stricmp(((self. __objectsDesc)[j]).GetObjectName(), objName) == 0):
				objIndex = (j)
				break

		if(objIndex == -1):
			raise RuntimeError(FormatMsg4("Object \"%1\" not found.\nLine: %2, column: %3.\nFile name: \"%4\".", self. __token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
		
		objDesc = (((self. __objectsDesc)[objIndex]))
		svs = (objDesc.GetSoftSkinVerts())
		
		numVerts = (self. __ReadInt())
		svs.SetNumVerts(numVerts)
		for i in range((0), (numVerts)):
			if(((i) & (15)) == 0):
				caption = ("Reading skin weights")
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			sv = (svs.GetVert(i))
			numWeights = (self. __ReadInt())
			sv.SetNumWeights(numWeights)
			for j in range((0), (numWeights)):
				objName = (self. __ReadString())
				objIndex = ( -1)
				for k in range((0), ((len(self. __objectsDesc)))):
					if(stricmp(((self. __objectsDesc)[k]).GetObjectName(), objName) == 0):
						objIndex = (k)
						break

				if(objIndex == -1):
					raise RuntimeError(FormatMsg4("Object \"%1\" not found.\nLine: %2, column: %3.\nFile name: \"%4\".", self. __token, (int_to_string(self. __tokenLine)), (int_to_string(self. __tokenColumn)), self. __filename))
				
				sv.SetBoneName(j, objName)
				sv.SetWeight(j, self. __ReadFloat())

		self. __ReadCertainToken("}")

	def __CalcAscType(self):
		if(self. __numMeshAnimations != 0):
			self. __ascType = (2)
		elif((self. __numMeshes == 1) and (self. __numBones == 0) and (self. __numSlots == 0)):
			self. __ascType = (1)
		elif(self. __numTMAnimations != 0):
			self. __ascType = (16)
		elif((self. __numSoftSkins != 0) or (self. __bip01Found)):
			self. __ascType = (8)
		else:
			self. __ascType = (4)

	def __SetSpaceTransform(self, spaceTransform):
		setupUnit = (spaceTransform.GetSetupUnit())
		if(setupUnit == 1):
			pass
		elif(setupUnit == 2):
			pass
		elif(setupUnit == 3):
			pass
		elif(setupUnit == 4):
			pass
		elif(setupUnit == 5):
			pass
		elif(setupUnit == 6):
			pass
		elif(setupUnit == 7):
			pass
		
		self. __spaceTransform = (spaceTransform)
		self. __scaleCoef = (spaceTransform.GetSystemUnitsPerFileUnit())

	def __SetModelPrefix(self, modelPrefix):
		self. __modelPrefix = (modelPrefix)

	def __SetSkinType(self, skinType):
		self. __skinType = (skinType)

	def __CreateMaterials(self):
		for i in range((0), ((len(self. __materialsDesc)))):
			mtlDescArray = (((self. __materialsDesc)[i]))
			mtlArray = ([])
			for j in range((0), ((len(mtlDescArray)))):
				mtlDesc = (((mtlDescArray)[j]))
				mtlName = (mtlDesc.GetMaterialName())
				caption = ("Creating materials")
				show_progress_bar(caption, 100 * i / (len(self. __materialsDesc)))
				mtl = (new_material(mtlName))
				set_diffuse_color(mtl, mtlDesc.GetDiffuseColor())
				set_diffuse_map_filename(mtl, mtlDesc.GetDiffuseMapFilename())
				mtlArray.append(mtl)
			
			self. __impMaterials.append(mtlArray)

	def __FindObjectByName(self, name):
		for i in range((0), ((len(self. __impObjectNames)))):
			if(stricmp(((self. __impObjectNames)[i]), name) == 0):
				return (((self. __impObjects)[i]))

		return ((find_object_by_name(self. __modelPrefix + name)))

	def __QuessBoneColor(self, objDesc):
		objName = (objDesc.GetObjectName())
		numChildren = (0)
		for i in range((0), ((len(self. __objectsDesc)))):
			if(((self. __objectsDesc)[i]).GetParentName() == objName):
				numChildren = (numChildren + 1)

		r = (127)
		g = (127)
		b = (127)
		
		if(stricmp((objName)[0:7], "Bip01 R") == 0):
			if(numChildren == 0):
				r = (113)
				g = (134)
				b = (6)
			else:
				r = (6)
				g = (134)
				b = (6)
			
		elif(stricmp((objName)[0:7], "Bip01 L") == 0):
			if(numChildren == 0):
				r = (134)
				g = (6)
				b = (6)
			else:
				r = (28)
				g = (28)
				b = (177)
			
		elif((stricmp(objName, "Bip01") == 0) or (stricmp(objName, "Bip01 Head") == 0)):
			r = (88)
			g = (143)
			b = (225)
		elif((stricmp(objName, "Bip01 Pelvis") == 0) or (stricmp(objName, "Bip01 Tail") == 0)):
			r = (224)
			g = (198)
			b = (87)
		elif((stricmp((objName)[0:11], "Bip01 Spine") == 0) or (stricmp((objName)[0:10], "Bip01 Neck") == 0)):
			r = (8)
			g = (110)
			b = (134)
		
		return (new_color(r / 255., g / 255., b / 255.))

	def __MaxDistToChild(self, objDesc, dir):
		tm = (objDesc.GetTransform())
		pos = (get_row3(tm))
		dirLen = (get_vector_length(dir))
		dist = (0)
		for i in range((0), ((len(self. __objectsDesc)))):
			childDesc = (((self. __objectsDesc)[i]))
			if(childDesc.GetParentName() == objDesc.GetObjectName()):
				if(childDesc.GetObjectType() == 1):
					childTM = (childDesc.GetTransform())
					childPos = (get_row3(childTM))
					h = (dot_product(dir, childPos - pos) / dirLen)
					if(dist < h):
						dist = (h)

		return (dist)

	def __CalcBoneBoundBox(self, objDesc, mode):
		defaultBoneSize = (10)
		if(mode == 1):
			self. __boneBoundBoxMinPoint = (new_vector( -defaultBoneSize/2, -defaultBoneSize/2, -defaultBoneSize/2))
			self. __boneBoundBoxMaxPoint = (new_vector(defaultBoneSize/2, defaultBoneSize/2, defaultBoneSize/2))
			
		elif(mode == 2):
			tm = (objDesc.GetTransform())
			dir_x = (get_row0(tm))
			dir_y = (get_row1(tm))
			dir_z = (get_row2(tm))
			
			along_x = (self. __MaxDistToChild(objDesc, dir_x))
			along_y = (self. __MaxDistToChild(objDesc, dir_y))
			along_z = (self. __MaxDistToChild(objDesc, dir_z))
			opposite_x = (0)
			opposite_y = (self. __MaxDistToChild(objDesc, -dir_y))
			opposite_z = (self. __MaxDistToChild(objDesc, -dir_z))
			
			if(along_x < defaultBoneSize):
				along_x = (defaultBoneSize)
			
			if(along_y < defaultBoneSize / 2):
				along_y = (defaultBoneSize / 2)
			
			if(along_z < defaultBoneSize / 2):
				along_z = (defaultBoneSize / 2)
			
			if(opposite_y < defaultBoneSize / 2):
				opposite_y = (defaultBoneSize / 2)
			
			if(opposite_z < defaultBoneSize / 2):
				opposite_z = (defaultBoneSize / 2)

			self. __boneBoundBoxMinPoint = (new_vector(0, -opposite_y, -opposite_z))
			self. __boneBoundBoxMaxPoint = (new_vector(along_x, along_y, along_z))

		self. __boneBoundBoxMinPoint = (self. __boneBoundBoxMinPoint * self. __scaleCoef)
		self. __boneBoundBoxMaxPoint = (self. __boneBoundBoxMaxPoint * self. __scaleCoef)

	def __CreateBones(self, mode):
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			objName = (objDesc.GetObjectName())
			if(objDesc.GetObjectType() == 1):
				caption = ("Creating bones")
				show_progress_bar(caption, 100 * i / (len(self. __objectsDesc)))
				newObjName = (self. __modelPrefix + objName)
				parentName = (objDesc.GetParentName())
				parentBone = (None)
				if(parentName != ""):
					parentBone = (self. __FindObjectByName(parentName))
					if(parentBone == None):
						parentBone = (None)

				self. __CalcBoneBoundBox(objDesc, mode)
				obj = (new_bone_object(newObjName, parentBone, self. __boneBoundBoxMinPoint, self. __boneBoundBoxMaxPoint, self. __skinType))
				tm = (objDesc.GetTransform())
				set_row3(tm, get_row3(tm) * self. __scaleCoef)
				set_transform(obj, tm)
				clr = (self. __QuessBoneColor(objDesc))
				set_wire_color(obj, clr)
				self. __impObjects.append(obj)
				self. __impObjectNames.append(objName)

	def __MakeOctahedronMesh(self, obj, a):
		msh = (MeshData(obj))
		msh.set_num_verts(6)
		(msh.set_vert(0, new_vector(0, 0, a)))
		(msh.set_vert(1, new_vector(a, 0, 0)))
		(msh.set_vert(2, new_vector(0, a, 0)))
		(msh.set_vert(3, new_vector( -a, 0, 0)))
		(msh.set_vert(4, new_vector(0, -a, 0)))
		(msh.set_vert(5, new_vector(0, 0, -a)))
		
		msh.set_num_faces(8)
		msh.set_face(0, new_face(0, 1, 2))
		msh.set_face(1, new_face(0, 2, 3))
		msh.set_face(2, new_face(0, 3, 4))
		msh.set_face(3, new_face(0, 4, 1))
		msh.set_face(4, new_face(5, 2, 1))
		msh.set_face(5, new_face(5, 3, 2))
		msh.set_face(6, new_face(5, 4, 3))
		msh.set_face(7, new_face(5, 1, 4))
		
		for i in range((0), ((msh.get_num_faces()))):
			for j in range((0), (3)):
				pass

		unusedArray = ((msh.update()))

	def __CreateSlots(self):
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			objName = (objDesc.GetObjectName())
			if(objDesc.GetObjectType() == 2):
				caption = ("Creating slots")
				show_progress_bar(caption, 100 * i / (len(self. __objectsDesc)))
				newObjName = (self. __modelPrefix + objName)
				obj = (new_mesh_object(newObjName))
				parentName = (objDesc.GetParentName())
				if(parentName != ""):
					parentObj = (self. __FindObjectByName(parentName))
					if(parentObj != None):
						set_parent(obj, parentObj)

				tm = (objDesc.GetTransform())
				set_row3(tm, get_row3(tm) * self. __scaleCoef)
				set_transform(obj, tm)
				self. __MakeOctahedronMesh(obj, (int_to_float(5)) * self. __scaleCoef)
				self. __impObjects.append(obj)
				self. __impObjectNames.append(objName)

	def __CreateMeshes(self):
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			objName = (objDesc.GetObjectName())
			if(objDesc.GetObjectType() == 3):
				caption = ("Creating meshes")
				show_progress_bar(caption, 100 * i / (len(self. __objectsDesc)))
				newObjName = (self. __modelPrefix + objName)
				if((find_object_by_name(newObjName)) != None):
					newObjName = ((unique_object_name(newObjName)))
				
				obj = (new_mesh_object(newObjName))
				parentName = (objDesc.GetParentName())
				if(parentName != ""):
					parentObj = (self. __FindObjectByName(parentName))
					if(parentObj != None):
						set_parent(obj, parentObj)

				tm = (objDesc.GetTransform())
				set_row3(tm, get_row3(tm) * self. __scaleCoef)
				set_transform(obj, tm)
				mshDesc = (objDesc.GetMeshDesc())
				msh = (MeshData(obj))
				numVerts = (mshDesc.GetNumVerts())
				msh.set_num_verts(numVerts)
				for j in range((0), (numVerts)):
					pt = (mshDesc.GetVert(j))
					pt = (pt * self. __scaleCoef)
					(msh.set_vert(j, pt))
				
				numFaces = (mshDesc.GetNumFaces())
				msh.set_num_faces(numFaces)
				matRef = (objDesc.GetMaterialRef())
				for j in range((0), (numFaces)):
					msh.set_face(j, mshDesc.GetFace(j))
					pass
					pass
					pass
					pass
					if((0 <= matRef) and (matRef < (len(self. __impMaterials)))):
						subMtls = (((self. __impMaterials)[matRef]))
						matIndex = (mshDesc.GetFaceMatID(j))
						numSubMtls = ((len(subMtls)))
						if(numSubMtls > 0):
							if(matIndex < 0):
								matIndex = (0)
							
							while(matIndex >= numSubMtls):
								matIndex = (matIndex - numSubMtls)
							
							mat = (((subMtls)[matIndex]))
							msh.set_face_material(j, mat)

				numTVerts = (mshDesc.GetNumTVerts())
				msh.set_num_tverts(numTVerts)
				for j in range((0), (numTVerts)):
					msh.set_tvert(j, mshDesc.GetTVert(j))
				
				numTVFaces = (mshDesc.GetNumTVFaces())
				msh.set_num_tvfaces(msh.get_num_faces())
				for j in range((0), (numTVFaces)):
					msh.set_tvface(j, mshDesc.GetTVFace(j))
				
				unusedArray = ((msh.update()))
				self. __impObjects.append(obj)
				self. __impObjectNames.append(objName)

	def __ReplaceBonesAndSlotsWithSampleMeshes(self, sampleMeshesDir):
		hide_progress_bar()
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			objName = (objDesc.GetObjectName())
			if((objDesc.GetObjectType() == 1) or (objDesc.GetObjectType() == 2)):
				obj = (self. __FindObjectByName(objName))
				if(obj != None):
					filename3ds = (sampleMeshesDir + objName + ".3ds")
					if(file_exists(filename3ds)):
						deselect_all()
						select(obj)
						call_importer("Krx3dsImp", filepath = filename3ds, quiet = True)
						obj = (self. __FindObjectByName(objName))
						if((obj != None) and (is_mesh_object(obj))):
							s = (self. __scaleCoef / 0.01)
							msh = (MeshData(obj))
							for j in range((0), ((msh.get_num_verts()))):
								(msh.set_vert(j, (msh.get_vert(j)) * s))
							
							unusedArray = ((msh.update()))

		self. __SetSpaceTransform(self. __spaceTransform)

	def __LinkModelToObject(self, newParentName):
		newParent = ((find_object_by_name(newParentName)))
		for i in range((0), ((len(self. __impObjects)))):
			obj = (((self. __impObjects)[i]))
			if(obj != None):
				if((get_parent(obj)) == None):
					set_parent(obj, newParent)
					set_transform(obj, multiply_matrix_matrix((get_transform(newParent)), (get_transform(obj))))

	def __SetDispProps(self, objType, layerName, dispProps):
		setVisibilityIndividually = (True)
		setRenderableIndividually = (True)
		setBoxModeIndividually = (True)
		setTransparentIndividually = (True)
		
		layer = (None)
		if(layer == None):
			layer = (None)

		if(layer != None):
			pass
			pass
			pass
			pass
			setVisibilityIndividually = (True != dispProps.GetVisibility())
			setRenderableIndividually = (True != dispProps.GetRenderable())
			setTransparentIndividually = (False != dispProps.GetTransparent())
			setBoxModeIndividually = (False != dispProps.GetBoxMode())

		for i in range((0), ((len(self. __impObjects)))):
			obj = (((self. __impObjects)[i]))
			objName = ((get_object_name(obj)))
			na = (AnalyzeName(objName))
			if(objType == na.GetObjectType()):
				if(layer != None):
					pass
				
				if(setVisibilityIndividually):
					show(obj, dispProps.GetVisibility())
				
				if(setRenderableIndividually):
					set_renderable(obj, dispProps.GetRenderable())
				
				if(setTransparentIndividually):
					set_transparent(obj, dispProps.GetTransparent())
				
				if(setBoxModeIndividually):
					set_box_mode(obj, dispProps.GetBoxMode())

	def __StoreModelPose(self):
		self. __modelBonesAndSlots = ([])
		self. __modelBonesAndSlotsStoredTMs = ([])
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			objName = (objDesc.GetObjectName())
			if((objDesc.GetObjectType() == 1) or (objDesc.GetObjectType() == 2)):
				obj = (self. __FindObjectByName(objName))
				if(obj != None):
					tm = ((get_transform(obj)))
					self. __modelBonesAndSlots.append(obj)
					self. __modelBonesAndSlotsStoredTMs.append(tm)

	def __InitialModelPose(self):
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			objName = (objDesc.GetObjectName())
			if((objDesc.GetObjectType() == 1) or (objDesc.GetObjectType() == 2)):
				obj = (self. __FindObjectByName(objName))
				if(obj != None):
					tm = (objDesc.GetTransform())
					set_row3(tm, get_row3(tm) * self. __scaleCoef)
					set_transform(obj, tm)

	def __RestoreModelPose(self):
		for i in range((0), ((len(self. __modelBonesAndSlots)))):
			obj = (((self. __modelBonesAndSlots)[i]))
			tm = (((self. __modelBonesAndSlotsStoredTMs)[i]))
			set_transform(obj, tm)

	def __ApplySingleSkin(self, objDesc):
		objName = (objDesc.GetObjectName())
		obj = (self. __FindObjectByName(objName))
		
		allBoneNames = ([])
		allBones = ([])
		svs = (objDesc.GetSoftSkinVerts())
		numVerts = (svs.GetNumVerts())
		
		for i in range((0), ((len(self. __objectsDesc)))):
			boneDesc = (((self. __objectsDesc)[i]))
			boneName = (boneDesc.GetObjectName())
			if(boneDesc.GetObjectType() == 1):
				found = (False)
				for j in range((0), (numVerts)):
					sv = (svs.GetVert(j))
					numWeights = (sv.GetNumWeights())
					for k in range((0), (numWeights)):
						if(sv.GetBoneName(k) == boneName):
							found = (True)
							break

					if(found):
						break

				if(found):
					bone = (self. __FindObjectByName(boneName))
					if(bone != None):
						allBoneNames.append(boneName)
						allBones.append(bone)

		if((len(allBones)) == 0):
			return

		SkinData(obj, self. __skinType)
		sd = (SkinData(obj))
		
		sd.add_bones(allBones)
		
		for j in range((0), (numVerts)):
			bones = ([])
			weights = ([])
			sv = (svs.GetVert(j))
			numWeights = (sv.GetNumWeights())
			totalWeight = (0)
			for k in range((0), (numWeights)):
				boneName = (sv.GetBoneName(k))
				weight = (sv.GetWeight(k))
				bon = (self. __FindObjectByName(boneName))
				if(bon != None):
					alreadyAppended = (False)
					for l in range((0), ((len(bones)))):
						if(((bones)[l]) == bon):
							weights[l] = (((weights)[l]) + weight)
							alreadyAppended = (True)

					if( not (alreadyAppended) and (weight != 0)):
						bones.append(bon)
						weights.append(weight)
					
					totalWeight = (totalWeight + weight)

			numWeights = ((len(bones)))
			for k in range((0), (numWeights)):
				weights[k] = (((weights)[k]) / totalWeight)
			
			sd.set_vert_weights(j, bones, weights)

	def __ApplySkin(self):
		caption = ("Applying skin")
		show_progress_bar(caption, 99)
		
		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			if((objDesc.GetSoftSkinVerts().GetNumVerts() != 0) and (objDesc.GetObjectType() == 3)):
				self. __ApplySingleSkin(objDesc)

	def __SetTimeTransform(self, timeTransform):
		self. __timeTransform = (timeTransform)

	def __ApplyAnimFrameRate(self):
		set_fps(self. __animFrameRate)

	def __IsTreeAnimated(self, startObj):
		children = ((get_children(startObj)))
		for i in range((0), ((len(children)))):
			child = (((children)[i]))
			childname = ((get_object_name(child)))
			if(has_tm_animation(child) or has_vertex_animation(child)):
				return (True)
			
			if(self. __IsTreeAnimated(child)):
				return (True)

		return (False)

	def __ApplyAnimRange(self):
		f = (self. __timeTransform.GetStartFrameInScene())
		l = (self. __timeTransform.GetEndFrameInScene())
		mergeAnimRanges = (self. __IsTreeAnimated(None))
		if(mergeAnimRanges):
			if(f > get_start_frame()):
				f = (get_start_frame())
			
			if(l < get_end_frame()):
				l = (get_end_frame())

		set_frame_range(f, l)

	def __TransformTime(self, frameInFile):
		a = (self. __timeTransform.GetStartFrameInFile())
		b = (self. __timeTransform.GetStartFrameInScene())
		frameInScene = (frameInFile - a + b)
		return (frameInScene)

	def __DelTreeTMAnimation(self, obj):
		children = ((get_children(obj)))
		for i in range((0), ((len(children)))):
			child = (((children)[i]))
			na = (AnalyzeName((get_object_name(child))))
			if(na.GetPrefix() == self. __modelPrefix):
				delete_tm_animation(child)
				self. __DelTreeTMAnimation(child)

	def __DeleteOldTMAnimation(self):
		self. __DelTreeTMAnimation(None)

	def __DelTreeMorphAnimation(self, obj):
		children = ((get_children(obj)))
		for i in range((0), ((len(children)))):
			child = (((children)[i]))
			na = (AnalyzeName((get_object_name(child))))
			if(na.GetPrefix() == self. __modelPrefix):
				delete_vertex_animation(child)
				self. __DelTreeMorphAnimation(child)

	def __DeleteOldMorphAnimation(self):
		self. __DelTreeMorphAnimation(None)

	def __DelTreeMeshes(self, obj):
		children = ((get_children(obj)))
		for i in range((0), ((len(children)))):
			child = (((children)[i]))
			na = (AnalyzeName((get_object_name(child))))
			if(na.GetPrefix() == self. __modelPrefix):
				self. __DelTreeMeshes(child)
				if(na.GetObjectType() == 3):
					delete_object(child)

	def __DeleteOldMeshes(self):
		self. __DelTreeMeshes(None)

	def __FindAnimatedObjects(self):
		self. __animatedOrHasAnimatedParent = ([])
		
		for i in range((0), ((len(self. __objectsDesc)))):
			self. __animatedOrHasAnimatedParent.append(False)

		for i in range((0), ((len(self. __objectsDesc)))):
			objDesc = (((self. __objectsDesc)[i]))
			if((objDesc.GetObjectType() == 1) or (objDesc.GetObjectType() == 2)):
				posTrack = (objDesc.GetPosTrack())
				rotTrack = (objDesc.GetRotTrack())
				if((posTrack.GetNumSamples() != 0) or (rotTrack.GetNumSamples() != 0)):
					self. __animatedOrHasAnimatedParent[i] = (True)
				else:
					parentName = (objDesc.GetParentName())
					if(parentName != ""):
						for k in range((0), (i)):
							if(((self. __objectsDesc)[k]).GetObjectName() == parentName):
								a = (((self. __animatedOrHasAnimatedParent)[k]))
								self. __animatedOrHasAnimatedParent[i] = (a)
								break

	def __CalcAnimationTMs(self, frameOffset):
		if((len(self. __animationTMs)) != (len(self. __objectsDesc))):
			self. __animationTMs = ([])
			
			for i in range((0), ((len(self. __objectsDesc)))):
				tm = (((self. __objectsDesc)[i]).GetTransform())
				set_row3(tm, get_row3(tm) * self. __scaleCoef)
				self. __animationTMs.append(tm)

		for i in range((0), ((len(self. __objectsDesc)))):
			if(((self. __animatedOrHasAnimatedParent)[i])):
				objDesc = (((self. __objectsDesc)[i]))
				tm = (objDesc.GetTransform())
				set_row3(tm, get_row3(tm) * self. __scaleCoef)
				
				parentName = (objDesc.GetParentName())
				if(parentName != ""):
					for k in range((0), (i)):
						if(((self. __objectsDesc)[k]).GetObjectName() == parentName):
							parentTM = (((self. __animationTMs)[k]))
							startParentTM = (((self. __objectsDesc)[k]).GetTransform())
							set_row3(startParentTM, get_row3(startParentTM) * self. __scaleCoef)
							tm = (multiply_matrix_matrix(multiply_matrix_matrix(tm, inverse_matrix(startParentTM)), parentTM))
							break

				posTrack = (objDesc.GetPosTrack())
				rotTrack = (objDesc.GetRotTrack())
				if((posTrack.GetNumSamples() != 0) or (rotTrack.GetNumSamples() != 0)):
					pos = (get_translation_part(tm))
					q = (get_rotation_part(tm))
					if(posTrack.GetNumSamples() != 0): 
						pos = (posTrack.GetSamplePos(frameOffset) * self. __scaleCoef)
					
					if(rotTrack.GetNumSamples() != 0):
						axis = (rotTrack.GetSampleAxis(frameOffset))
						angle = (rotTrack.GetSampleAngle(frameOffset))
						q = (new_quaternion(axis, angle))
					
					tm = (multiply_matrix_matrix(rotation_matrix(q), translation_matrix(pos)))

				self. __animationTMs[i] = (tm)

	def __ApplySingleTMAnim(self):
		for i in range((0), ((len(self. __objectsDesc)))):
			if(((self. __animatedOrHasAnimatedParent)[i])):
				objDesc = (((self. __objectsDesc)[i]))
				objName = (objDesc.GetObjectName())
				obj = (self. __FindObjectByName(objName))
				if(obj != None):
					tm = (((self. __animationTMs)[i]))
					parentTM = (identity_matrix())
					parentName = (objDesc.GetParentName())
					if(parentName != ""):
						for k in range((0), (i)):
							if(((self. __objectsDesc)[k]).GetObjectName() == parentName):
								parentTM = (((self. __animationTMs)[k]))
								break

					ancestorTM = (identity_matrix())
					ancestor = ((get_parent(obj)))
					while(ancestor != None):
						na = (AnalyzeName((get_object_name(ancestor))))
						if(na.GetObjectType() != 3):
							ancestorTM = ((get_transform(ancestor)))
							break
						
						ancestor = ((get_parent(ancestor)))
					
					tm2 = (multiply_matrix_matrix(multiply_matrix_matrix(tm, inverse_matrix(parentTM)), ancestorTM))
					
					tm3 = ((get_transform(obj)))
					tmDelta = (tm2 - tm3)
					delta0 = (get_vector_length(get_row0(tmDelta)))
					delta1 = (get_vector_length(get_row1(tmDelta)))
					delta2 = (get_vector_length(get_row2(tmDelta)))
					delta3 = (get_vector_length(get_row3(tmDelta)))
					
					eps = (1 / (int_to_float(1000)))
					if((delta0 > eps) or (delta1 > eps) or (delta2 > eps) or (delta3 > eps * self. __scaleCoef)):
						animate_tm(obj, tm2)	

	def __ApplyTMAnimation(self):
		self. __FindAnimatedObjects()
		start = (self. __timeTransform.GetStartFrameInFile())
		end = (self. __timeTransform.GetEndFrameInFile())
		for frameInFile in range((start), (end + 1)):
			frameInScene = (self. __TransformTime(frameInFile))
			set_current_frame(frameInScene)
			caption = ("Applying TM animation")
			show_progress_bar(caption, 100 * (frameInFile - start) / (end - start + 1))
			self. __CalcAnimationTMs(frameInFile - self. __animStartFrame)
			self. __ApplySingleTMAnim()
		
		set_current_frame(self. __timeTransform.GetStartFrameInScene())

	def __ApplyMorphAnimation(self):
		start = (self. __timeTransform.GetStartFrameInFile())
		end = (self. __timeTransform.GetEndFrameInFile())
		for frameInFile in range((start), (end + 1)):
			frameInScene = (self. __TransformTime(frameInFile))
			set_current_frame(frameInScene)
			caption = ("Applying morph animation")
			show_progress_bar(caption, 100 * (frameInFile - start) / (end - start + 1))
			for i in range((0), ((len(self. __objectsDesc)))):
				objDesc = (((self. __objectsDesc)[i]))
				objName = (objDesc.GetObjectName())
				morphTrack = (objDesc.GetMorphTrack())
				obj = (self. __FindObjectByName(objName))
				if(obj != None):
					matWorldToLocal = (inverse_matrix(objDesc.GetTransform()))
					points = (morphTrack.GetSampleVerts(frameInFile - self. __animStartFrame))
					for k in range((0), ((len(points)))):
						pos = (((points)[k]))
						pos = (multiply_vector_matrix(pos, matWorldToLocal))
						pos = (pos * self. __scaleCoef)
						points[k] = (pos)
					
					animate_vertices(obj, points)

		set_current_frame(self. __timeTransform.GetStartFrameInScene())

	def Init(self):
		self. __file = (NewFile())
		self. __filename = ("")
		self. __lineText = ("")
		self. __line = (0)
		self. __column = (0)
		self. __tokenLine = (0)
		self. __tokenColumn = (0)
		self. __token = ("")
		self. __token1 = ("")
		self. __token2 = ("")
		self. __token3 = ("")
		self. __tokenEOF = ("EOF")
		self. __animStartFrame = (0)
		self. __animEndFrame = (100)
		self. __animFrameRate = (25)
		self. __materialsDesc = ([])
		self. __objectsDesc = ([])
		self. __numBones = (0)
		self. __bip01Found = (False)
		self. __numSlots = (0)
		self. __numMeshes = (0)
		self. __numTMAnimations = (0)
		self. __numMeshAnimations = (0)
		self. __numSoftSkins = (0)
		self. __ascType = (4)
		self. __spaceTransform = (NewSpaceTransform())
		self. __scaleCoef = (1.0)
		self. __timeTransform = (NewTimeTransform())
		self. __modelPrefix = ("")
		self. __modelBonesAndSlots = ([])
		self. __modelBonesAndSlotsStoredTMs = ([])
		self. __animatedOrHasAnimatedParent = ([])
		self. __animationTMs = ([])
		self. __impMaterials = ([])
		self. __impObjects = ([])
		self. __impObjectNames = ([])

	def ReadASCFile(self, filename):
		self. Init()
		self. __filename = (filename)
		try:
			self. __file.Open(filename, "rt")
			level = (0)
			token = ("")
			
			token = (self. __ReadToken())
			if(stricmp("*3DSMAX_ASCIIEXPORT", token) != 0):
				raise RuntimeError(FormatMsg1("File is not an ASCII model.\nFile name: \"%1\".", self. __filename))
			
			self. __ReadASCVersion()
			
			while(True):
				if(level == 0):
					token = (self. __ReadTokenSafe())
					if(token == self. __tokenEOF):
						break
					
				else:
					token = (self. __ReadToken())
				
				if(stricmp(token, "*COMMENT") == 0):
					self. __ReadComment()
				elif(stricmp(token, "*SCENE") == 0):
					self. __ReadSceneInfo()
				elif(stricmp(token, "*MATERIAL_LIST") == 0):
					self. __ReadMaterialList()
				elif(stricmp(token, "*GEOMOBJECT") == 0):
					self. __ReadGeomObject()
				elif(stricmp(token, "*HELPEROBJECT") == 0):
					self. __ReadGeomObject()
				elif(stricmp(token, "*MESH_SOFTSKINVERTS") == 0):
					self. __ReadSoftSkinVerts()
				elif(token == "{"):
					level = (level + 1)
				elif(token == "}"):
					level = (level - 1)

			self. __file.Close()
			self. __CalcAscType()
		
		except RuntimeError as ex:
			self. __file.Close()
			raise

	def GetAnimStartFrame(self):
		return (self. __animStartFrame)

	def GetAnimEndFrame(self):
		return (self. __animEndFrame)

	def GetAnimFrameRate(self):
		return (self. __animFrameRate)

	def GetNumMeshes(self):
		return (self. __numMeshes)

	def GetNumSlots(self):
		return (self. __numSlots)

	def GetNumBones(self):
		return (self. __numBones)

	def GetAscType(self):
		return (self. __ascType)

	def GetObjectsDesc(self):
		return (self. __objectsDesc)

	def SetSpaceTransform(self, spaceTransform):
		self. __SetSpaceTransform(spaceTransform)

	def SetModelPrefix(self, modelPrefix):
		self. __SetModelPrefix(modelPrefix)

	def SetSkinType(self, skinType):
		self. __SetSkinType(skinType)

	def CreateMaterials(self):
		self. __CreateMaterials()

	def CreateCubicBones(self):
		self. __CreateBones(1)

	def CreateConnectedBones(self):
		self. __CreateBones(2)

	def CreateSlots(self):
		self. __CreateSlots()

	def CreateMeshes(self):
		self. __CreateMeshes()

	def ReplaceBonesAndSlotsWithSampleMeshes(self, sampleMeshesDir):
		self. __ReplaceBonesAndSlotsWithSampleMeshes(sampleMeshesDir)

	def SetDispProps(self, objType, layerName, dispProps):
		self. __SetDispProps(objType, layerName, dispProps)

	def LinkModelToObject(self, newParentName):
		self. __LinkModelToObject(newParentName)

	def StoreModelPose(self):
		self. __StoreModelPose()

	def InitialModelPose(self):
		self. __InitialModelPose()

	def ApplySkin(self):
		self. __ApplySkin()

	def RestoreModelPose(self):
		self. __RestoreModelPose()

	def SetTimeTransform(self, timeTransform):
		self. __SetTimeTransform(timeTransform)

	def ApplyAnimFrameRate(self):
		self. __ApplyAnimFrameRate()

	def ApplyAnimRange(self):
		self. __ApplyAnimRange()

	def DeleteOldTMAnimation(self):
		self. __DeleteOldTMAnimation()

	def DeleteOldMorphAnimation(self):
		self. __DeleteOldMorphAnimation()

	def DeleteOldMeshes(self):
		self. __DeleteOldMeshes()

	def ApplyTMAnimation(self):
		self. __ApplyTMAnimation()

	def ApplyMorphAnimation(self):
		self. __ApplyMorphAnimation()

def NewASCFileLoader():
	loader = (TASCFileLoader())
	loader.Init()
	return (loader)

class TASCImporterDlgInput:
	def Init(self):
		self. __importFileName = ("")
		self. __importFileSize = (0)
		self. __ascType = (4)
		self. __curSceneMode = (1)
		self. __curAnimMode = (1)
		self. __supportedSkinTypes = ([])
		self. __slots = ([])
		self. __selectedSlot = ("")
		self. __bones = ([])
		self. __selectedBone = ("")
		self. __prefixes = ([])
		self. __uniquePrefix = ("")
		self. __selectedPrefix = ("")
		self. __numFileMeshes = (0)
		self. __numFileSlots = (0)
		self. __numFileBones = (0)
		self. __animStartFrame = (0)
		self. __animEndFrame = (100)
		self. __animFrameRate = (25)
		self. __sampleMeshesDir = ("")
		self. __minFrame = (1)
		self. __maxFrame = (1000)
		self. __systemUnitsPerFileUnit = (1)

	def Write(self, f):
		f.WriteString(self. __importFileName)
		f.WriteUnsignedLong(self. __importFileSize)
		f.WriteUnsignedChar(self. __ascType)
		f.WriteUnsignedChar(self. __curSceneMode)
		f.WriteUnsignedChar(self. __curAnimMode)
		f.WriteUnsignedLong((len(self. __supportedSkinTypes)))
		for i in range((0), ((len(self. __supportedSkinTypes)))):
			f.WriteString(((self. __supportedSkinTypes)[i]))
		
		f.WriteUnsignedLong((len(self. __slots)))
		for i in range((0), ((len(self. __slots)))):
			f.WriteString(((self. __slots)[i]))
		
		f.WriteString(self. __selectedSlot)
		f.WriteUnsignedLong((len(self. __bones)))
		for j in range((0), ((len(self. __bones)))):
			f.WriteString(((self. __bones)[j]))
		
		f.WriteString(self. __selectedBone)
		f.WriteUnsignedLong((len(self. __prefixes)))
		for j in range((0), ((len(self. __prefixes)))):
			f.WriteString(((self. __prefixes)[j]))
		
		f.WriteString(self. __uniquePrefix)
		f.WriteString(self. __selectedPrefix)
		f.WriteUnsignedLong(self. __numFileMeshes)
		f.WriteUnsignedLong(self. __numFileSlots)
		f.WriteUnsignedLong(self. __numFileBones)
		f.WriteSignedLong(self. __animStartFrame)
		f.WriteSignedLong(self. __animEndFrame)
		f.WriteUnsignedLong(self. __animFrameRate)
		f.WriteString(self. __sampleMeshesDir)
		f.WriteSignedLong(self. __minFrame)
		f.WriteSignedLong(self. __maxFrame)
		f.WriteFloat(self. __systemUnitsPerFileUnit)

	def Read(self, f):
		self. __importFileName = (f.ReadString())
		self. __importFileSize = (f.ReadUnsignedLong())
		self. __ascType = (f.ReadUnsignedChar())
		self. __curSceneMode = (f.ReadUnsignedChar())
		self. __curAnimMode = (f.ReadUnsignedChar())
		
		numSupportedSkinTypes = (f.ReadUnsignedLong())
		self. __supportedSkinTypes = ([])
		for i in range((0), (numSupportedSkinTypes)):
			self. __supportedSkinTypes.append(f.ReadString())

		numSlots = (f.ReadUnsignedLong())
		self. __slots = ([])
		
		for i in range((0), (numSlots)):
			self. __slots.append(f.ReadString())
		
		self. __selectedSlot = (f.ReadString())
		
		numBones = (f.ReadUnsignedLong())
		self. __bones = ([])
		
		for j in range((0), (numBones)):
			self. __bones.append(f.ReadString())
		
		self. __selectedBone = (f.ReadString())
		
		numPrefixes = (f.ReadUnsignedLong())
		self. __prefixes = ([])
		
		for j in range((0), (numPrefixes)):
			self. __prefixes.append(f.ReadString())
		
		self. __uniquePrefix = (f.ReadString())
		self. __selectedPrefix = (f.ReadString())
		
		self. __numFileMeshes = (f.ReadUnsignedLong())
		self. __numFileSlots = (f.ReadUnsignedLong())
		self. __numFileBones = (f.ReadUnsignedLong())
		self. __animStartFrame = (f.ReadSignedLong())
		self. __animEndFrame = (f.ReadSignedLong())
		self. __animFrameRate = (f.ReadUnsignedLong())
		self. __sampleMeshesDir = (f.ReadString())
		self. __minFrame = (f.ReadSignedLong())
		self. __maxFrame = (f.ReadSignedLong())
		self. __systemUnitsPerFileUnit = (f.ReadFloat())

	def SetImportFileName(self, importFileName):
		self. __importFileName = (importFileName)

	def GetImportFileName(self):
		return (self. __importFileName)

	def SetImportFileSize(self, importFileSize):
		self. __importFileSize = (importFileSize)

	def GetImportFileSize(self):
		return (self. __importFileSize)

	def SetAscType(self, ascType):
		self. __ascType = (ascType)

	def GetAscType(self):
		return (self. __ascType)

	def SetCurrentSceneMode(self, curSceneMode):
		self. __curSceneMode = (curSceneMode)

	def GetCurrentSceneMode(self):
		return (self. __curSceneMode)

	def SetCurrentAnimationMode(self, curAnimMode):
		self. __curAnimMode = (curAnimMode)

	def GetCurrentAnimationMode(self):
		return (self. __curAnimMode)

	def SetSupportedSkinTypes(self, supportedSkinTypes):
		self. __supportedSkinTypes = (supportedSkinTypes)

	def GetSupportedSkinTypes(self):
		return (self. __supportedSkinTypes)

	def SetSlots(self, slots):
		self. __slots = (slots)

	def GetSlots(self):
		return (self. __slots)

	def SetSelectedSlot(self, selectedSlot):
		self. __selectedSlot = (selectedSlot)

	def GetSelectedSlot(self):
		return (self. __selectedSlot)

	def SetBones(self, bones):
		self. __bones = (bones)

	def GetBones(self):
		return (self. __bones)

	def SetSelectedBone(self, selectedBone):
		self. __selectedBone = (selectedBone)

	def GetSelectedBone(self):
		return (self. __selectedBone)

	def SetPrefixes(self, prefixes):
		self. __prefixes = (prefixes)

	def GetPrefixes(self):
		return (self. __prefixes)

	def SetUniquePrefix(self, uniquePrefix):
		self. __uniquePrefix = (uniquePrefix)

	def GetUniquePrefix(self):
		return (self. __uniquePrefix)

	def SetSelectedPrefix(self, selectedPrefix):
		self. __selectedPrefix = (selectedPrefix)

	def GetSelectedPrefix(self):
		return (self. __selectedPrefix)

	def SetNumFileMeshes(self, numFileMeshes):
		self. __numFileMeshes = (numFileMeshes)

	def GetNumFileMeshes(self):
		return (self. __numFileMeshes)

	def SetNumFileSlots(self, numFileSlots):
		self. __numFileSlots = (numFileSlots)

	def GetNumFileSlots(self):
		return (self. __numFileSlots)

	def SetNumFileBones(self, numFileBones):
		self. __numFileBones = (numFileBones)

	def GetNumFileBones(self):
		return (self. __numFileBones)

	def SetAnimStartFrame(self, frameIndex):
		self. __animStartFrame = (frameIndex)

	def GetAnimStartFrame(self):
		return (self. __animStartFrame)

	def SetAnimEndFrame(self, frameIndex):
		self. __animEndFrame = (frameIndex)

	def GetAnimEndFrame(self):
		return (self. __animEndFrame)

	def SetAnimFrameRate(self, frameRate):
		self. __animFrameRate = (frameRate)

	def GetAnimFrameRate(self):
		return (self. __animFrameRate)

	def SetSampleMeshesDir(self, sampleMeshesDir):
		self. __sampleMeshesDir = (sampleMeshesDir)

	def GetSampleMeshesDir(self):
		return (self. __sampleMeshesDir)

	def SetMinFrame(self, frameIndex):
		self. __minFrame = (frameIndex)

	def GetMinFrame(self):
		return (self. __minFrame)

	def SetMaxFrame(self, frameIndex):
		self. __maxFrame = (frameIndex)

	def GetMaxFrame(self):
		return (self. __maxFrame)

	def SetSystemUnitsPerFileUnit(self, scaleCoef):
		self. __systemUnitsPerFileUnit = (scaleCoef)

	def GetSystemUnitsPerFileUnit(self):
		return (self. __systemUnitsPerFileUnit)

def NewASCImporterDlgInput():
	dlgInput = (TASCImporterDlgInput())
	dlgInput.Init()
	return (dlgInput)

class TASCImporterDlgOutput:
	def Init(self, dlgInput):
		self. __curSceneMode = (dlgInput.GetCurrentSceneMode())
		self. __curAnimMode = (dlgInput.GetCurrentAnimationMode())
		self. __connectBones = (True)
		self. __useSampleMeshes = (True)
		self. __sampleMeshesDir = (dlgInput.GetSampleMeshesDir())
		self. __selectedSlot = (dlgInput.GetSelectedSlot())
		self. __selectedBone = (dlgInput.GetSelectedBone())
		self. __selectedPrefix = (dlgInput.GetSelectedPrefix())
		self. __continueImport = (True)
		
		self. __selectedSkinType = ("")
		supportedSkinTypes = (dlgInput.GetSupportedSkinTypes())
		if((len(supportedSkinTypes)) != 0):
			self. __selectedSkinType = (((supportedSkinTypes)[0]))

		self. __spaceTransform = (NewSpaceTransform())
		self. __spaceTransform.SetSystemUnitsPerFileUnit(dlgInput.GetSystemUnitsPerFileUnit())
		
		self. __timeTransform = (NewTimeTransform())
		self. __timeTransform.SetMinFrameInFile(dlgInput.GetAnimStartFrame())
		self. __timeTransform.SetMaxFrameInFile(dlgInput.GetAnimEndFrame())
		self. __timeTransform.SetMinFrameInScene(dlgInput.GetMinFrame())
		self. __timeTransform.SetMaxFrameInScene(dlgInput.GetMaxFrame())
		self. __timeTransform.SetStartFrameInFile(dlgInput.GetAnimStartFrame())
		self. __timeTransform.SetEndFrameInFile(dlgInput.GetAnimEndFrame())
		frameOffset = (0)
		if(dlgInput.GetAnimStartFrame() < dlgInput.GetMinFrame()):
			frameOffset = (1000)
		
		self. __timeTransform.SetStartFrameInScene(dlgInput.GetAnimStartFrame() + frameOffset)
		self. __timeTransform.SetEndFrameInScene(dlgInput.GetAnimEndFrame() + frameOffset)

	def Write(self, f):
		f.WriteUnsignedChar(self. __curSceneMode)
		f.WriteUnsignedChar(self. __curAnimMode)
		f.WriteString(self. __selectedSkinType)
		f.WriteBool(self. __connectBones)
		f.WriteBool(self. __useSampleMeshes)
		f.WriteString(self. __sampleMeshesDir)
		f.WriteString(self. __selectedSlot)
		f.WriteString(self. __selectedBone)
		f.WriteString(self. __selectedPrefix)
		self. __spaceTransform.Write(f)
		self. __timeTransform.Write(f)
		f.WriteBool(self. __continueImport)

	def Read(self, f):
		self. __curSceneMode = (f.ReadUnsignedChar())
		self. __curAnimMode = (f.ReadUnsignedChar())
		self. __selectedSkinType = (f.ReadString())
		self. __connectBones = (f.ReadBool())
		self. __useSampleMeshes = (f.ReadBool())
		self. __sampleMeshesDir = (f.ReadString())
		self. __selectedSlot = (f.ReadString())
		self. __selectedBone = (f.ReadString())
		self. __selectedPrefix = (f.ReadString())
		self. __spaceTransform.Read(f)
		self. __timeTransform.Read(f)
		self. __continueImport = (f.ReadBool())

	def SetCurrentSceneMode(self, curSceneMode):
		self. __curSceneMode = (curSceneMode)

	def GetCurrentSceneMode(self):
		return (self. __curSceneMode)

	def SetCurrentAnimationMode(self, curAnimMode):
		self. __curAnimMode = (curAnimMode)

	def GetCurrentAnimationMode(self):
		return (self. __curAnimMode)

	def SetSelectedSkinType(self, selectedSkinType):
		self. __selectedSkinType = (selectedSkinType)

	def GetSelectedSkinType(self):
		return (self. __selectedSkinType)

	def SetConnectBones(self, connectBones):
		self. __connectBones = (connectBones)

	def GetConnectBones(self):
		return (self. __connectBones)

	def SetUseSampleMeshes(self, useSampleMeshes):
		self. __useSampleMeshes = (useSampleMeshes)

	def GetUseSampleMeshes(self):
		return (self. __useSampleMeshes)

	def SetSampleMeshesDir(self, sampleMeshesDir):
		self. __sampleMeshesDir = (sampleMeshesDir)

	def GetSampleMeshesDir(self):
		return (self. __sampleMeshesDir)

	def SetSelectedSlot(self, selectedSlot):
		self. __selectedSlot = (selectedSlot)

	def GetSelectedSlot(self):
		return (self. __selectedSlot)

	def SetSelectedBone(self, selectedBone):
		self. __selectedBone = (selectedBone)

	def GetSelectedBone(self):
		return (self. __selectedBone)

	def SetSelectedPrefix(self, selectedPrefix):
		self. __selectedPrefix = (selectedPrefix)

	def GetSelectedPrefix(self):
		return (self. __selectedPrefix)

	def SetTimeTransform(self, timeTransform):
		self. __timeTransform = (timeTransform)

	def GetTimeTransform(self):
		return (self. __timeTransform)

	def SetSpaceTransform(self, spaceTransform):
		self. __spaceTransform = (spaceTransform)

	def GetSpaceTransform(self):
		return (self. __spaceTransform)

	def SetContinueImport(self, continueImport):
		self. __continueImport = (continueImport)

	def GetContinueImport(self):
		return (self. __continueImport)

def NewASCImporterDlgOutput(dlgInput):
	dlgOutput = (TASCImporterDlgOutput())
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
	register_importer("KrxAscImp", "ASC", "Kerrax ASCII Model") 

def unregister(): 
	unregister_importer("KrxAscImp") 

if __name__ == "main": 
	register() 

def KrxAscImp(filename_param, quiet_param = False): 
	QUIET = quiet_param 
	IMPORT_FILE_NAME = filename_param 
	IMPORT_FILE_SIZE = get_file_size(IMPORT_FILE_NAME)
	
	loader = (NewASCFileLoader())
	try:
		loader.ReadASCFile(IMPORT_FILE_NAME)
	
	except RuntimeError as ex:
		show_error_box("Kerrax ASC Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
		hide_progress_bar()
		return (0)

	dlgInput = (NewASCImporterDlgInput())
	dlgInput.SetImportFileName(IMPORT_FILE_NAME)
	dlgInput.SetImportFileSize(IMPORT_FILE_SIZE)
	
	ascType = (loader.GetAscType())
	dlgInput.SetAscType(ascType)
	
	dlgInput.SetNumFileMeshes(loader.GetNumMeshes())
	dlgInput.SetNumFileSlots(loader.GetNumSlots())
	dlgInput.SetNumFileBones(loader.GetNumBones())
	dlgInput.SetAnimStartFrame(loader.GetAnimStartFrame())
	dlgInput.SetAnimEndFrame(loader.GetAnimEndFrame())
	dlgInput.SetAnimFrameRate(loader.GetAnimFrameRate())
	
	dlgInput.SetSampleMeshesDir((get_root_dir() + "KrxImpExp\\sample_meshes\\"))
	
	dlgInput.SetMinFrame(get_min_frame())
	dlgInput.SetMaxFrame(get_max_frame())
	dlgInput.SetSystemUnitsPerFileUnit(0.01)
	
	dlgInput.SetUniquePrefix("")
	dlgInput.SetSelectedPrefix("")
	dlgInput.SetPrefixes([])
	dlgInput.SetCurrentSceneMode(1)
	
	sceneAnalyzer = (AnalyzeScene())
	
	if(ascType == 4):
		dlgInput.SetUniquePrefix(sceneAnalyzer.GetUniquePrefix())
	elif(ascType == 1):
		dlgInput.SetUniquePrefix(sceneAnalyzer.GetUniquePrefix())
	elif(ascType == 2):
		sceneAnalyzer.FindAppropriateMorphMeshes(loader.GetObjectsDesc())
		dlgInput.SetPrefixes(sceneAnalyzer.GetAppropriatePrefixes())
		selectedPrefixes = (sceneAnalyzer.GetSelectedAppropriatePrefixes())
		if((len(selectedPrefixes)) != 0):
			dlgInput.SetSelectedPrefix(((selectedPrefixes)[0]))
		
	elif(ascType == 8):
		sceneAnalyzer.FindAppropriateDynamicModels(loader.GetObjectsDesc())
		dlgInput.SetUniquePrefix(sceneAnalyzer.GetUniquePrefix())
		dlgInput.SetPrefixes(sceneAnalyzer.GetScenePrefixes())
		
		selectedPrefixes = (sceneAnalyzer.GetSelectedAppropriatePrefixes())
		if(((len(selectedPrefixes)) == 0) and ((len(sceneAnalyzer.GetAppropriatePrefixes())) != 0)):
			selectedPrefixes.append(((sceneAnalyzer.GetAppropriatePrefixes())[0]))
		
		if((len(selectedPrefixes)) != 0):
			dlgInput.SetSelectedPrefix(((selectedPrefixes)[0]))
			dlgInput.SetCurrentSceneMode(5)
		
	elif(ascType == 16):
		sceneAnalyzer.FindAppropriateDynamicModels(loader.GetObjectsDesc())
		dlgInput.SetPrefixes(sceneAnalyzer.GetScenePrefixes())
		
		selectedPrefixes = (sceneAnalyzer.GetSelectedAppropriatePrefixes())
		if(((len(selectedPrefixes)) == 0) and ((len(sceneAnalyzer.GetAppropriatePrefixes())) != 0)):
			selectedPrefixes.append(((sceneAnalyzer.GetAppropriatePrefixes())[0]))
		
		if((len(selectedPrefixes)) != 0):
			dlgInput.SetSelectedPrefix(((selectedPrefixes)[0]))

	if(ascType == 1):
		dlgInput.SetSlots(sceneAnalyzer.GetSceneSlots())
		selectedSlots = (sceneAnalyzer.GetSelectedSlots())
		if((len(selectedSlots)) != 0):
			dlgInput.SetSelectedSlot(((selectedSlots)[0]))
			dlgInput.SetCurrentSceneMode(3)
		
		dlgInput.SetBones(sceneAnalyzer.GetSceneBones())
		selectedBones = (sceneAnalyzer.GetSelectedBones())
		if((len(selectedBones)) != 0):
			dlgInput.SetSelectedBone(((selectedBones)[0]))
			dlgInput.SetCurrentSceneMode(4)

	if(ascType == 8):
		dlgInput.SetSupportedSkinTypes(get_supported_skin_types())

	if(ascType == 2 or ascType == 16):
		dlgInput.SetAnimStartFrame(loader.GetAnimStartFrame())
		dlgInput.SetAnimEndFrame(loader.GetAnimEndFrame())
		dlgInput.SetAnimFrameRate(loader.GetAnimFrameRate())

	dlgOutput = (NewASCImporterDlgOutput(dlgInput))
	inputFile = (NewFile())
	outputFile = (NewFile())
	if( not (QUIET)):
		try:
			show_progress_bar("Showing dialog", 0)
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("ASCImporterDlgInput")
			dlgInput.Write(inputFile)
			inputFile.Close()
			RunUIExe()
			outputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgOutput.bin"), "rb")
			structName = (outputFile.ReadString())
			dlgOutput.Read(outputFile)
			outputFile.Close()
			
			if( not (dlgOutput.GetContinueImport())):
				hide_progress_bar()
				return (2)

		except RuntimeError as ex:
			inputFile.Close()
			outputFile.Close()
			show_error_box("Kerrax ASC Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
			hide_progress_bar()
			return (2)

	loader.SetModelPrefix(dlgOutput.GetSelectedPrefix())
	loader.SetSkinType(dlgOutput.GetSelectedSkinType())
	
	if(ascType == 4):
		curSceneMode = (dlgOutput.GetCurrentSceneMode())
		if(curSceneMode == 1):
			reset_scene()

		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.SetSpaceTransform(spaceTransform)
		
		if(dlgOutput.GetConnectBones()):
			loader.CreateConnectedBones()
		else:
			loader.CreateCubicBones()
		
		loader.CreateSlots()
		if(dlgOutput.GetUseSampleMeshes()):
			loader.ReplaceBonesAndSlotsWithSampleMeshes(dlgOutput.GetSampleMeshesDir())

		loader.CreateMaterials()
		loader.CreateMeshes()
		
		dispProps = (NewObjectDisplayProps())
		dispProps.SetRenderable(False)
		loader.SetDispProps(1, "Bones", dispProps)
		
		dispProps.SetTransparent(True)
		loader.SetDispProps(2, "Slots", dispProps)
		
		dispProps.Reset()
		loader.SetDispProps(3, "Meshes", dispProps)
		
	elif(ascType == 1):
		curSceneMode = (dlgOutput.GetCurrentSceneMode())
		if(curSceneMode == 1):
			reset_scene()

		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.SetSpaceTransform(spaceTransform)
		
		loader.CreateMaterials()
		loader.CreateMeshes()
		
		dispProps = (NewObjectDisplayProps())
		loader.SetDispProps(3, "Meshes", dispProps)
		
		if(curSceneMode == 3):
			loader.LinkModelToObject(dlgOutput.GetSelectedSlot())
		elif(curSceneMode == 4):
			loader.LinkModelToObject(dlgOutput.GetSelectedBone())

	elif(ascType == 2):
		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.SetSpaceTransform(spaceTransform)
		timeTransform = (dlgOutput.GetTimeTransform())
		loader.SetTimeTransform(timeTransform)
		
		curAnimMode = (dlgOutput.GetCurrentAnimationMode())
		if(curAnimMode == 1):
			loader.DeleteOldMorphAnimation()
		
		loader.ApplyAnimFrameRate()
		loader.ApplyAnimRange()
		loader.ApplyMorphAnimation()
		
	elif(ascType == 8):
		curSceneMode = (dlgOutput.GetCurrentSceneMode())
		if(curSceneMode == 1):
			reset_scene()

		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.SetSpaceTransform(spaceTransform)
		
		if((curSceneMode != 5) and (curSceneMode != 6)):
			if(dlgOutput.GetConnectBones()):
				loader.CreateConnectedBones()
			else:
				loader.CreateCubicBones()
			
			loader.CreateSlots()
			if(dlgOutput.GetUseSampleMeshes()):
				loader.ReplaceBonesAndSlotsWithSampleMeshes(dlgOutput.GetSampleMeshesDir())

		if(curSceneMode == 5):
			loader.DeleteOldMeshes()

		if((curSceneMode == 5) or (curSceneMode == 6)):
			loader.StoreModelPose()
			loader.InitialModelPose()

		loader.CreateMaterials()
		loader.CreateMeshes()
		loader.ApplySkin()
		
		if((curSceneMode == 5) or (curSceneMode == 6)):
			loader.RestoreModelPose()

		dispProps = (NewObjectDisplayProps())
		dispProps.SetRenderable(False)
		loader.SetDispProps(1, "Bones", dispProps)
		loader.SetDispProps(2, "Slots", dispProps)
		
		dispProps.SetRenderable(True)
		dispProps.SetTransparent(True)
		loader.SetDispProps(3, "Meshes", dispProps)	
		
	elif(ascType == 16):
		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.SetSpaceTransform(spaceTransform)
		timeTransform = (dlgOutput.GetTimeTransform())
		loader.SetTimeTransform(timeTransform)
		
		curAnimMode = (dlgOutput.GetCurrentAnimationMode())
		if(curAnimMode == 1):
			loader.DeleteOldTMAnimation()

		loader.ApplyAnimFrameRate()
		loader.ApplyAnimRange()
		loader.ApplyTMAnimation()

	hide_progress_bar()
	return (1)


