#include "Pawn.h"
#include "Constants.h"
#include <iostream>
using namespace std;

namespace edit
{
	Pawn::Pawn(IrrlichtDevice* D)
		: device(D), smgr(D->getSceneManager()), driver(D->getVideoDriver())
	{
		parent = smgr->addEmptySceneNode();

		body = smgr->addAnimatedMeshSceneNode(smgr->getMesh(body_male),
			parent,-1,core::vector3df(),core::vector3df(),core::vector3df(1),true);
		body->setMaterialFlag(video::EMF_LIGHTING, false);

		scene::IBoneSceneNode* bone = body->getJointNode("Bip01_Head");
		core::vector3df pos = bone->getAbsolutePosition();
		pos.Y *= -1;
		head = smgr->addAnimatedMeshSceneNode(smgr->getMesh(defaulthead),
			parent,-1,pos,core::vector3df(0),core::vector3df(1),true);
		head->setMaterialFlag(video::EMF_LIGHTING, false);

		armor = smgr->addAnimatedMeshSceneNode(0,parent,-1,core::vector3df(),core::vector3df(),core::vector3df(1),true);
	}

	void Pawn::SetGender(GENDER newgender)
	{
		if (gender==newgender)
			return;

		gender = newgender;
		video::ITexture* oldtex = body->getMaterial(0).getTexture(0);	//save texture

		body->remove();
		const char* name = 0;
		if (gender==MALE)
			name = body_male;
		else
			name = body_female;

		body = smgr->addAnimatedMeshSceneNode(smgr->getMesh(name),
			parent,-1,core::vector3df(),core::vector3df(),core::vector3df(1),true);
		body->setMaterialFlag(video::EMF_LIGHTING, false);

		scene::IBoneSceneNode* bone = body->getJointNode("Bip01_Head");
		core::vector3df pos = bone->getAbsolutePosition();
		pos.Y *= -1;

		body->setMaterialTexture(0, oldtex);
		

		head->setPosition(pos);
	}
	void Pawn::SetBodyTexture(u32 num)
	{
		char buffer[512];
		sprintf_s(buffer, 512, "Hum_Body_Naked_V%i_C0.tga", num);
		body->setMaterialTexture(0, driver->getTexture(buffer));
	}
	void Pawn::SetHeadTexture(u32 num)
	{
		char buffer[512];
		sprintf_s(buffer, 512, "Hum_Head_V%i_C0.tga", num);
		head->setMaterialTexture(0, driver->getTexture(buffer));
	}
	void Pawn::SetHeadMesh(const core::stringw& name)
	{
		core::stringw hlpstr = name;
		hlpstr+=".x";

		video::ITexture* oldtex = head->getMaterial(0).getTexture(0);	//save texture
		head->remove();
		
		scene::IBoneSceneNode* bone = body->getJointNode("Bip01_Head");
		core::vector3df pos = bone->getAbsolutePosition();
		pos.Y *= -1;
		head = smgr->addAnimatedMeshSceneNode(smgr->getMesh(hlpstr.c_str()),
			parent,-1,pos,core::vector3df(0),core::vector3df(1),true);
		head->setMaterialFlag(video::EMF_LIGHTING, false);
		head->setMaterialTexture(0, oldtex);
	}
	void Pawn::SetModelScale(const core::vector3df& s)
	{
		//currentscale.X += fatness/10;
		parent->setScale(s);
	}
	void Pawn::SetArmor(const wchar_t* name)
	{
		core::stringw hlpstr = name;
		hlpstr+=".x";

		armor->remove();
		
		armor = smgr->addAnimatedMeshSceneNode(smgr->getMesh(hlpstr.c_str()),parent,-1,core::vector3df(),core::vector3df(),core::vector3df(1),true);
		armor->setMaterialFlag(video::EMF_LIGHTING, false);
	}

	GENDER Pawn::GetGender() const
	{
		return gender;
	}
}