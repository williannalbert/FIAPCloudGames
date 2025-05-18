﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class UserIdDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid UserId { get; set; }
}
