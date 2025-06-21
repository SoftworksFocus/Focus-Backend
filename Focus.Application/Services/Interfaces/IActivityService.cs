using Focus.Application.DTO.Activity;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IActivityService : IService<Activity, GetActivityDto, CreateActivityDto, UpdateActivityDto>;