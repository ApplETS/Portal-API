using PortalApi.Models.FirestoreModels;
using System;
using System.Collections.Generic;
using WhatsNewApi.Models.FirestoreModels;

namespace PortalUnitTest.Mock;
public static class Constants
{
    // MODELS
    public static Project ValidProject = new Project()
    {
        Id = "1",
        CurrentVersion = "3.0.0",
        Name = "ETSMobile"
    };

    public static IEnumerable<Project> ValidProjectsList = new Project[]
    {
        ValidProject,
        ValidProject
    };

    public static WhatsNewPage ValidWhatsNewPage = new WhatsNewPage()
    {
        Title = new InternationalizedText { En = "valid title", Fr = "titre valide" },
        Description = new InternationalizedText { En = "valid description", Fr = "description valide" },
        Color = "#FFFFFF",
        MediaUrl = "https://img.com/validurl"

    };

    public static WhatsNew ValidWhatsNew = new WhatsNew()
    {
        Id = "1",
        ProjectId = "1",
        Version = "3.0.1",
        Pages = new List<WhatsNewPage>
        {
            ValidWhatsNewPage
        }
    };

    public static WhatsNew ValidWhatsNew2 = new WhatsNew()
    {
        Id = "1",
        ProjectId = "1",
        Version = "3.0.5",
        Pages = new List<WhatsNewPage>
        {
            ValidWhatsNewPage
        }
    };

    public static IEnumerable<WhatsNew> ValidWhatsNewList = new WhatsNew[]
    {
        ValidWhatsNew,
        ValidWhatsNew2
    };

    //DTOS
    public static WhatsNewCreationDTO ValidWhatsNewDto1 = new WhatsNewCreationDTO()
    {
        Version = "3.0.1",
        Pages = new WhatsNewPageCreationDTO[]
        {
            new WhatsNewPageCreationDTO()
            {
                Title = new InternationalizedText { En = "valid title", Fr = "titre valide" },
                Description = new InternationalizedText { En = "valid description", Fr = "description valide" },
                Color = "#FFFFFF",
                MediaUrl = "https://img.com/test"
            },
            new WhatsNewPageCreationDTO()
            {

                Title = new InternationalizedText { En = "valid title", Fr = "titre valide" },
                Description = new InternationalizedText { En = "valid description", Fr = "description valide" },
                Color = "#FFFFFF",
                MediaUrl = "https://img.com/test"
            }
        }
    };

    public static WhatsNewCreationDTO InvalidWhatsNewDto1 = new WhatsNewCreationDTO()
    {
        Version = "invalid",
        Pages = new WhatsNewPageCreationDTO[]
        {
            new WhatsNewPageCreationDTO()
            {
                Title = new InternationalizedText { En = "valid title", Fr = "titre valide" },
                Description = new InternationalizedText { En = "valid description", Fr = "description valide" },
                Color = "#FFFFFF",
                MediaUrl = "https://img.com/test"
            }
        }
    };

    public static ProjectCreationDTO ValidProjectDto = new ProjectCreationDTO()
    {
        Name = "ETSMobile",
        CurrentVersion = "3.0.0"
    };

    public static ProjectCreationDTO InvalidProjectDto = new ProjectCreationDTO()
    {
        Name = "",
        CurrentVersion = ""
    };

    public static ProjectUpdateDTO ValidProjectUpdateDto = new ProjectUpdateDTO()
    {
        CurrentVersion = "3.0.0",
    };

    public static ProjectUpdateDTO InvalidProjectUpdateDto = new ProjectUpdateDTO()
    {
        CurrentVersion = "",
    };
}
