using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SizeDto
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class UpdateSizeDto
    {
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
    }
  
    public class SizeTypeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SizeDto> Sizes { get; set; }
    }


    public class UpdateSizeTypeDto
    {
        public string Name { get; set; }
        public List<UpdateSizeDto> Sizes { get; set; }
    }

    public class CreateSizeTypeDto
    {
        public string Name { get; set; }
        public List<CreateSizeDto> Sizes { get; set; }
    }

    public class CreateSizeDto
    {
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class AddNewSizeDto
    {
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
        public string SizeTypeId { get; set; }
    }
}
