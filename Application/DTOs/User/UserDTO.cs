﻿using Application.DTOs.Library;
using Application.DTOs.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ApplicationUserId { get; set; }
    public bool IsActive { get; set; }
    public LibraryDTO Library { get; set; }
    public WalletDTO Wallet { get; set; }
}
