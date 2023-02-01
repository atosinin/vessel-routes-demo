namespace DotNetApi.Models
{
    public static class WhateverExtensions
    {
        public static void CreateFromWhateverDTO(this Whatever whatever, WhateverDTO whateverDTO)
        {
            whatever.UpdateFromWhateverDTO(whateverDTO, whateverDTO.CreatedBy);
        }

        public static void UpdateFromWhateverDTO(this Whatever whatever, WhateverDTO whateverDTO, string updatedBy)
        {
            whatever.Name = whateverDTO.Name;
            whatever.Description = whateverDTO.Description;
            whatever.UpdateLastModified(updatedBy);
        }
    }
}
