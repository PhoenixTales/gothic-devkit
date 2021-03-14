#ifndef __GOTHIC_EDIT_PAWN_H__
#define __GOTHIC_EDIT_PAWN_H__
#include "Constants.h"

namespace edit
{
	using namespace irr;

	class Pawn
	{
	private:
		IrrlichtDevice* device;
		scene::ISceneManager* smgr;
		video::IVideoDriver* driver;
		f32 fatness;


		GENDER gender;

		scene::IAnimatedMeshSceneNode* body;
		scene::IAnimatedMeshSceneNode* head;
		scene::IAnimatedMeshSceneNode* armor;
		scene::ISceneNode* parent;

		//scene::IMeshSceneNode* meleeweapon;
		//scene::IMeshSceneNode* rangedweapon;

	public:
		Pawn(IrrlichtDevice*);
		void SetBodyTexture(u32 num);
		void SetHeadTexture(u32 num);
		void SetGender(GENDER newgender);
		void SetHeadMesh(const core::stringw& name);
		void SetModelScale(const core::vector3df& s);

		void SetArmor(const wchar_t* name);

		GENDER GetGender() const;
	};
}


#endif