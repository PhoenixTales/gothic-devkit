#ifndef __GOTHIC_EDIT_FILTER_H__
#define __GOTHIC_EDIT_FILTER_H__
#include "Pawn.h"

namespace edit
{
	using namespace irr;

	struct ListItem	//In Meshlist are skincolor and id not used
	{
		core::stringw name;
		SKINCOLOR skincolor;
		GENDER gender;
		u32 id;

		ListItem() : name(L""), gender(MALE), id(0), skincolor(SKINCOLOR_NORMAL)	{}
	};

	class List
	{
	private:
		core::array<ListItem> items;

	public:
		core::array<ListItem> GetItemsFiltered(GENDER g, SKINCOLOR sk) const
		{
			core::array<ListItem> arr;
			for (u32 i=0; i<items.size(); ++i)
			{
				if (items[i].name!="" && items[i].gender==g && (items[i].skincolor==sk || sk==SKINCOLOR_ALL))
					arr.push_back(items[i]);
			}
			return arr;
		}
		core::array<ListItem> GetItemsFiltered(GENDER g) const
		{
			core::array<ListItem> arr;
			for (u32 i=0; i<items.size(); ++i)
			{
				if (items[i].name!=L"" && items[i].gender==g)
					arr.push_back(items[i]);
			}
			return arr;
		}
		core::array<ListItem> GetAllItems() const
		{
			return items;
		}
		void Add(ListItem& item)
		{
			items.push_back(item);
			//printf("Name=\"%s\" Gender=%i Skincolor=%i\n", item.name.c_str(), item.gender, item.skincolor);
		}
		ListItem* GetItem(const core::stringw& name)
		{
			for (u32 i=0; i<items.size(); ++i)
				if (name.equals_ignore_case(items[i].name))
					return &items[i];
			return 0;
		}
	};
}


#endif