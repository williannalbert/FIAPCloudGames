﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class GamePromotionRepository(AppDbContext context) : Repository<GamePromotion>(context), IGamePromotionRepository
{
}
