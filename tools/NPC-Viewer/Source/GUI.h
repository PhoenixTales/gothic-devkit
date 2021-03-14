#ifndef __GOTHIC_EDIT_GUI_
#define __GOTHIC_EDIT_GUI_
#include "Irrlicht.h"
#include "Lists.h"

namespace edit
{
	using namespace irr;


	enum E_GUIELEMENT_NUMBERS
	{
		EGN_GENDER = 30000,
		EGN_SKINCOLOR,
		EGN_HEADMESH,
		EGN_BODYTEXTURE,
		EGN_HEADTEXTURE,

		EGN_SCALE_X,
		EGN_SCALE_Y,
		EGN_SCALE_Z
	};

	class EventReceiver;

	class GUI
	{
	private:
		irr::io::IAttributes* attrib;

		List& headmeshlist;
		List& headtexlist;
		List& bodytexlist;
		gui::IGUIEnvironment *guienv;

		gui::IGUIComboBox* ctrl_gender;
		gui::IGUIComboBox* ctrl_skincolor;

		gui::IGUIComboBox* ctrl_headmesh;
		gui::IGUIComboBox* ctrl_bodytexture;
		gui::IGUIComboBox* ctrl_headtexture;

		gui::IGUIScrollBar* ctrl_scale_x;
		gui::IGUIScrollBar* ctrl_scale_y;
		gui::IGUIScrollBar* ctrl_scale_z;

		gui::IGUIStaticText* ctrl_scale_x_text;
		gui::IGUIStaticText* ctrl_scale_y_text;
		gui::IGUIStaticText* ctrl_scale_z_text;

		EventReceiver* eventreceiver;

		GENDER gender;
		SKINCOLOR skincolor;

		void Update();

	public:
		GUI(EventReceiver* evt, gui::IGUIEnvironment *env, List& hml, List& htl, List& btl, irr::io::IAttributes* attr);
		~GUI();

		bool OnEvent(const irr::SEvent::SGUIEvent& event);
	};
}

#endif