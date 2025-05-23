﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class UpdateUserDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public bool IsActive { get; set; }
}
