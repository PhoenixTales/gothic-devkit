#ifndef __GOTHIC_EDIT_EVENT_
#define __GOTHIC_EDIT_EVENT_
#include <Irrlicht.h>
#include "Lists.h"
#include "GUI.h"

namespace edit
{
	class Pawn;

	using namespace irr;

	class EventReceiver : public IEventReceiver
	{
	private:
		IrrlichtDevice* device;
		Pawn* pawn;
		GUI* Gui;
		irr::io::IAttributes* attrib;
		irr::io::IWriteFile *logfile;

		//List<core::stringc> headmeshlist;
		//List<u32> headtexlist;
		//List<u32> bodytexlist;
		List headmeshlist;
		List headtexlist;
		List bodytexlist;

		gui::IGUIWindow* window;
		gui::IGUIComboBox* ctrl_gender;
		gui::IGUIEditBox* ctrl_headmesh;
		gui::IGUIButton* ctrl_headmesh_submit;
		gui::IGUISpinBox* ctrl_bodytexture;
		gui::IGUISpinBox* ctrl_headtexture;
		gui::IGUIScrollBar* ctrl_scaleX;
		gui::IGUIScrollBar* ctrl_scaleY;
		gui::IGUIScrollBar* ctrl_scaleZ;


		bool keys[irr::KEY_KEY_CODES_COUNT];

		scene::ICameraSceneNode *camera;

		u32 lasttime;
		core::vector2df rotation;


		SKINCOLOR TextToSkincolor(const char* skincolor) const;
		GENDER TextToGender(const char* gender) const;
		void LoadLists();

	public:
		EventReceiver(irr::IrrlichtDevice*, Pawn*, irr::io::IAttributes*);
		~EventReceiver();

		virtual bool OnEvent(const SEvent&);

		void Set(GENDER g, const core::stringw& headmeshname, const core::stringw& headtexname, const core::stringw& bodytexname);
		void SetScale(const core::vector3df& scale);

		void OnRender();
	};
}

#endif