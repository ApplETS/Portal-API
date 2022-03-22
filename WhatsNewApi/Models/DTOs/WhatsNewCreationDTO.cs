
namespace WhatsNewApi.Models.DTOs;
public class WhatsNewCreationDTO
{
	public string? Version { get; set; }
	public IEnumerable<WhatsNewPageCreationDTO>? Pages { get; set; }
}