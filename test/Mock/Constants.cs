using System;
using System.Collections.Generic;
using WhatsNewApi.Models.FirestoreModels;

namespace PortalUnitTest.Mock;
public static class Constants
{
	// MODELS
	public static Project validProject = new Project()
    {
		Id = "1",
		CurrentVersion = "3.0.0",
		Name = "ETSMobile"
    };

    public static IEnumerable<Project> validProjectsList = new Project[]
    {
        validProject,
        validProject
    };

	public static WhatsNewPage validWhatsNewPage = new WhatsNewPage()
	{
		Title = "valid title",
		Description = "valid desc",
		Color = "#FFFFFF",
		MediaUrl = "https://img.com/validurl"

	};

	public static WhatsNew validWhatsNew = new WhatsNew()
	{
		Id = "1",
		ProjectId = "1",
		Version = "3.0.1",
		Pages = new List<WhatsNewPage>
		{
            validWhatsNewPage
		}
	};

	public static WhatsNew validWhatsNew2 = new WhatsNew()
	{
		Id = "1",
		ProjectId = "1",
		Version = "3.0.5",
		Pages = new List<WhatsNewPage>
		{
			validWhatsNewPage
		}
	};

	public static IEnumerable<WhatsNew> validWhatsNewList = new WhatsNew[]
	{
		validWhatsNew,
		validWhatsNew2
	};

	//DTOS
	public static WhatsNewCreationDTO validDto1 = new WhatsNewCreationDTO()
	{
		Version = "3.0.1",
		Pages = new WhatsNewPageCreationDTO[]
		{
			new WhatsNewPageCreationDTO()
            {
				Title = "title1",
				Description = "desc1",
				Color = "#FFFFFF",
				MediaUrl = "https://img.com/test"
            },
			new WhatsNewPageCreationDTO()
			{
				Title = "title2",
				Description = "desc2",
				Color = "#FFFFFF",
				MediaUrl = "https://img.com/test"
			}
		}
	};
}


