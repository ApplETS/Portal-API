using PortalApi.Models.FirestoreModels;

namespace WhatsNewApi.Models.DTOs;

public class WhatsNewPageCreationDTO
{
    public InternationalizedText? Title { get; set; }
    public InternationalizedText? Description { get; set; }
    public string? MediaUrl { get; set; }
    public string? Color { get; set; }
}
