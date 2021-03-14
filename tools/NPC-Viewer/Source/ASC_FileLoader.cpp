#include "ASC_FileLoader.h"
using namespace irr;

#include <Assert.h>
#include <stdio.h>
#include <Windows.h>


ASC_FileLoader::ASC_FileLoader(IrrlichtDevice* D)
	: device(D), filesystem(0), P(0), buffer(0), end(false), smgr(0), mesh(0)
{
	assert(device);
	filesystem = device->getFileSystem();
	smgr = device->getSceneManager();
}


scene::IAnimatedMesh* ASC_FileLoader::loadFile(irr::io::path filename)
{
	io::IReadFile* file = filesystem->createAndOpenFile(filename);

	if (!file)
	{
		core::stringc hlp = "Can not open file ";
		hlp += filename.c_str();
		device->getLogger()->log(hlp.c_str(), ELL_ERROR);
		return 0;
	}

	scene::IAnimatedMesh* animmesh = loadFile(file);

	file->drop();
	return animmesh;
}

scene::IAnimatedMesh* ASC_FileLoader::loadFile(irr::io::IReadFile* file)
{
	if (!file)
	{
		device->getLogger()->log("Can not read file", ELL_ERROR);
		return 0;
	}

	buffer = new char[file->getSize()+1];
	file->seek(0);		//reset cursor
	file->read(buffer, file->getSize());
	buffer[file->getSize()] = 0;

	P = buffer;

	if (mesh)
		mesh->drop();

	mesh = new scene::SMesh;

	core::stringc token = getNextToken();
	while (token.size())
	{
		if (token=="*3DSMAX_ASCIIEXPORT" || token=="*COMMENT")
			gotoNextLine();

		else if (token=="*MATERIAL_LIST")
		{
			parseDataObjectMaterialList();
			gotoNextLine();
		}
		else if (token=="*GEOMOBJECT")
		{
			parseDataObjectGeom();
		}
		else
		{
			device->getLogger()->log((core::stringc("ASC_MeshFileReader: unknown token\t\"")+token+"\"").c_str());
			parseUnknownDataObject();
			//gotoNextLine();
		}

		token = getNextToken();
	}

	delete[] buffer;
	buffer = 0;
	P = 0;


	for (u32 i=0; i<mesh->MeshBuffers.size(); ++i)
	{
		mesh->MeshBuffers[i]->recalculateBoundingBox();
	}


	scene::SAnimatedMesh* animmesh = new scene::SAnimatedMesh;
	mesh->recalculateBoundingBox();

	animmesh->addMesh(mesh);
	animmesh->Type = scene::EAMT_UNKNOWN;
	animmesh->recalculateBoundingBox();
	mesh->drop();
	mesh = 0;

	device->getLogger()->log("Mesh loaded");
	end = false;

	return animmesh;
}

void ASC_FileLoader::parseDataObjectMaterialList()
{
	device->getLogger()->log("ASC_MeshFileReader: Reading '*MATERIALLIST ");

	gotoNextLine();

	core::stringc token = getNextToken();

	while (token.size())
	{
		if (token=="}")
		{
			//gotoNextLine();
			return;
		}
		else if (token=="*MATERIAL_COUNT")
		{
			materials.reallocate(readInt());
			gotoNextLine();
		}
		else if (token=="*MATERIAL")
			parseDataObjectMaterial();

		else
		{
			//puts((core::stringc("ASC_MeshFileReader: unknown token\t\"")+token+"\"").c_str());
			parseUnknownDataObject();
			//gotoNextLine();
		}

		token = getNextToken();
	}
}



void ASC_FileLoader::parseDataObjectMaterial()
{
	core::stringc str = "ASC_MeshFileReader: Reading '*MATERIAL ";
	str += readInt();
	str += "'";

	device->getLogger()->log(str.c_str());

	video::SMaterial mat;
	core::stringc name;

	core::stringc token = getNextToken();

	while (token.size() && token!="}")
	{
		if (token=="*MATERIAL_NAME")
			name = readString();
		else if (token=="*MATERIAL_CLASS")
			gotoNextLine();
		else if (token=="*MATERIAL_AMBIENT")
			mat.AmbientColor = readColor();
		else if (token=="*MATERIAL_DIFFUSE")
			mat.DiffuseColor = readColor();
		else if (token=="*MATERIAL_SPECULAR")
			mat.SpecularColor = readColor();
		else if (token=="*MATERIAL_SHINE")
			mat.Shininess = readFloat();
		else if (token=="*NUMSUBMTLS")
		{
			gotoNextLine();
		}
		else if (token=="*SUBMATERIAL")
		{
			u32 count=readInt();
			parseDataObjectSubMaterial(mat, count);
		}
		else
			gotoNextLine();
		
		token = getNextToken();
	}

	materials.push_back(mat);
	gotoNextLine();
}

void ASC_FileLoader::parseDataObjectSubMaterial(video::SMaterial& mat, u8 count)
{
	core::stringc str = "ASC_MeshFileReader: Reading '*SUBMATERIAL ";
	str += count;
	str += "'";
	device->getLogger()->log(str.c_str());


	core::stringc token = getNextToken();

	bool bracket_once = false;

	while (token.size())
	{
		if (token=="}" && bracket_once)
			break;
		else if (token=="}")
			bracket_once = true;

		if (token=="*BITMAP")
		{
			video::ITexture* tex = device->getVideoDriver()->getTexture(readString());
			//mat.setTexture(count, tex);
			mat.setTexture(count, tex);
		}

		gotoNextLine();
		token = getNextToken();
	}
	gotoNextLine();
}



void ASC_FileLoader::parseDataObjectGeom()
{
	device->getLogger()->log("ASC_MeshFileReader: Reading '*GEOMOBJECT'");

	gotoNextLine();		//Erstes { auslassen
	core::stringc token = getNextToken();

	u32 brackets = 0;

	scene::SMeshBuffer* meshbuffer = new scene::SMeshBuffer;

	while (token.size())
	{
		if (token=="}" && brackets <= 0)
			break;
		else if (token=="}")
			--brackets;
		else if (token == "{")
			++brackets;

		else if (token=="*MATERIAL_REF")
		{
			u32 i=readInt();
			if (materials.size()>i)
			{
				meshbuffer->Material.TextureLayer[0] = materials[i].TextureLayer[0];
			}
		}
		else if (token=="*MESH_SOFTSKIN" || token=="*MESH")
			parseMesh(meshbuffer);

		token = getNextToken();
	}

	if (meshbuffer->getIndexCount() != 0 && meshbuffer->getVertexCount() != 0)
	{	
		mesh->addMeshBuffer(meshbuffer);
		meshbuffer->drop();
	}
}

void ASC_FileLoader::parseMesh(scene::SMeshBuffer* meshbuffer)
{
	device->getLogger()->log("ASC_MeshFileReader: Reading mesh");

	u32 brackets = 0;

	core::stringc token = "***";	//Damit size nicht 0 ist

	core::array<video::S3DVertex> vertices;
	core::array<u16> indices;
	core::array<core::vector3df> texturecoords;
	core::array<u16> textureindices;
	//indices und textureindices haben die gleiche Größe

	while (token.size())
	{
		if (token=="}" && brackets>0)
			--brackets;
		else if (token=="}")	//äußere Klammer
			break;
		else if (token=="{")
			++brackets;


		if (token=="*MESH_NUMVERTEX")
		{
			//meshbuffer->Vertices.set_used(readInt());
			vertices.set_used(readInt());
		}
		else if (token=="*MESH_NUMFACES")
		{
			//meshbuffer->Indices.set_used(readInt()*3);
			indices.set_used(readInt()*3);
		}
		else if (token=="*MESH_VERTEX_LIST")
		{
			while (getNextToken()!="}")
			{
				u32 i= readInt();
				//meshbuffer->Vertices[i].Pos = readVector3df();
				vertices[i].Pos = readVector3df();
			}
		}
		else if (token=="*MESH_FACE_LIST")
		{
			while (token!="}")
			{
				u32 count = readInt(); //Überspringe Zeilenanfang und hole Polygonnummer

				//meshbuffer->Indices[count*3 + 0] = readInt();
				//meshbuffer->Indices[count*3 + 1] = readInt();
				//meshbuffer->Indices[count*3 + 2] = readInt();

				indices[count*3 + 0] = readInt();
				indices[count*3 + 1] = readInt();
				indices[count*3 + 2] = readInt();

				//printf("%i	A: %i	B: %i	C: %i", count, meshbuffer->Indices[count*3 + 0], meshbuffer->Indices[count*3 + 1], meshbuffer->Indices[count*3 + 2]);

				gotoNextLine();
				token = getNextToken();
			}
		}
		if (token=="*MESH_NUMTVERTEX")
		{
			texturecoords.set_used(readInt());
		}
		if (token=="*MESH_NUMTVFACES")
		{
			textureindices.set_used(readInt()*3);
		}
		else if (token=="*MESH_TVERTLIST")
		{
			while (getNextToken()!="}")
			{
				u32 i= readInt();
				texturecoords[i] = readVector3df();
			}
		}
		else if (token=="*MESH_TFACELIST")
		{
			while (token!="}")
			{
				u32 count = readInt(); //Überspringe Zeilenanfang und hole Polygonnummer

				textureindices[count*3 + 0] = readInt();
				textureindices[count*3 + 1] = readInt();
				textureindices[count*3 + 2] = readInt();
				

				gotoNextLine();
				token = getNextToken();
			}
		}


		token = getNextToken();
	}

	meshbuffer->Indices=textureindices;
	meshbuffer->Vertices.set_used(texturecoords.size());


	//Sortieren und in meshbuffer schreiben:
	for (u32 i=0; i<indices.size(); ++i)
	{
		meshbuffer->Vertices[textureindices[i]].Pos = vertices[indices[i]].Pos;
		meshbuffer->Vertices[textureindices[i]].Normal = vertices[indices[i]].Normal;
		//meshbuffer->Vertices[textureindices[i]].TCoords.set(texturecoords[textureindices[i]].X, texturecoords[textureindices[i]].Y);
		meshbuffer->Vertices[textureindices[i]].TCoords.set(texturecoords[textureindices[i]].X, texturecoords[textureindices[i]].Y);
	}
}





void ASC_FileLoader::parseUnknownDataObject()
{
	while (P[0]!='{')
	{
		if (P[0]=='\n')
			return;
		++P;
	}
	++P;
	
	core::stringc token = "***";	//Damit size größer als 0 ist
	u32 brackets = 1;

	while (token.size() && brackets != 0)
	{
		token = getNextToken();

		if (token=="}")
			--brackets;
		else if (token == "{")
			++brackets;
	}
}


core::stringc ASC_FileLoader::getNextToken()
{
	gotoNextNoneWhiteSpace();

	if (end)
		return "";

	if ( P[0]=='"' )
		return core::stringc("\"") + readString() + core::stringc("\"");

	if (P[0]=='}' || P[0]=='{')
		return core::stringc(P++, 1);

	const char* P2 = P;

	while ( !core::isspace(P[0]) )
	{
		if (P[0]==0)
		{
			end = true;
			return "";
		}
		++P;
	}
	return core::stringc(P2, (int)P-(int)P2);
}

void ASC_FileLoader::gotoNextNumber()
{
	if (end)
		return;

	while ( (*P)!='-' && !core::isdigit(*P) )
	{
		if ( (*P)==0 )
		{
			end = true;
			return;
		}
		++P;
	}
}
void ASC_FileLoader::gotoNextNoneWhiteSpace()
{
	if (end)
		return;

	while ( core::isspace(P[0]) )
	{
		if ( P[0]==0 )
		{
			end = true;
			return;
		}
		++P;
	}
}
void ASC_FileLoader::gotoNextLine()
{
	if (end)
		return;

	while ( (*P)!='\n' && (*P)!='\r' )
	{
		if ( (*P)==0 )
		{
			end = true;
			return;
		}
		++P;
	}
	++P;
}

s32 ASC_FileLoader::readInt()
{
	gotoNextNumber();

	if (end)
		return 0;

	return core::strtol10(P, &P);
}

f32 ASC_FileLoader::readFloat()
{
	gotoNextNumber();
	
	if (end)
		return 0.0f;

	f32 ftmp;
	P = core::fast_atof_move(P, ftmp);
	return ftmp;
}

core::stringc ASC_FileLoader::readString()
{
	gotoNextNoneWhiteSpace();

	
	if (end)
		return "";

	if (P[0] != '"')
		return "";

	++P;

	const char* P_2=P;

	while (P[0]!='"')
	{
		if (P[0]==0)
		{
			end = true;
			return "";
		}
		++P;
	}
	++P;

	return core::stringc(P_2, (int)P-(int)P_2-1);
}

core::vector3df ASC_FileLoader::readVector3df()		//Z und Y sind getauscht
{
	//return core::vector3df(readFloat(), readFloat(), readFloat());
	f32 X = readFloat();
	f32 Z = readFloat();
	f32 Y = readFloat();
	return core::vector3df(X, Y, Z);
}
core::vector2df ASC_FileLoader::readVector2df()
{
	return core::vector2df(readFloat(), readFloat());
}
video::SColor ASC_FileLoader::readColor()
{
	video::SColorf tmpColor(readFloat(), readFloat(), readFloat());
	return tmpColor.toSColor();
	return true;
}