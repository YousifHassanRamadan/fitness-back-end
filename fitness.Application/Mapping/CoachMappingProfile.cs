using AutoMapper;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Features.Clients.DTOs;
using fitness.Domain.Entities;

namespace fitness.Application.Mapping;

public class CoachMappingProfile : Profile
{
    public CoachMappingProfile()
    {
        // Coach mappings
        CreateMap<Coach, CoachProfileDto>()
            .ForMember(
                dest => dest.ClientsCount,
                opt => opt.MapFrom(src => src.Clients.Count));

        CreateMap<Client, CoachClientDto>()
            .ForMember(
                dest => dest.Gender,
                opt => opt.MapFrom(src => src.Gender.ToString()));

        // Client mappings
        CreateMap<Client, ClientProfileDto>()
            .ForMember(
                dest => dest.Gender,
                opt => opt.MapFrom(src => src.Gender.ToString()));

        CreateMap<Coach, ClientCoachDto>();




        CreateMap<WorkoutPlanAssignment, WorkoutPlanDto>()
    .ForMember(dest => dest.Id,
        opt => opt.MapFrom(src => src.WorkoutPlan.Id))
    .ForMember(dest => dest.Name,
        opt => opt.MapFrom(src => src.WorkoutPlan.Name))
    .ForMember(dest => dest.Description,
        opt => opt.MapFrom(src => src.WorkoutPlan.Description))
    .ForMember(dest => dest.DurationInWeeks,
        opt => opt.MapFrom(src => src.WorkoutPlan.DurationInWeeks))
    .ForMember(dest => dest.WorkoutDays,
        opt => opt.MapFrom(src => src.WorkoutPlan.WorkoutDays));

        CreateMap<WorkoutDay, WorkoutDayDto>()
            .ForMember(dest => dest.Exercises,
                opt => opt.MapFrom(src => src.WorkoutExercises));

        CreateMap<WorkoutExercise, WorkoutExerciseDto>()
            .ForMember(dest => dest.ExerciseName,
                opt => opt.MapFrom(src => src.Exercise.Name))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Exercise.Description))
            .ForMember(dest => dest.MuscleGroup,
                opt => opt.MapFrom(src => src.Exercise.MuscleGroup.ToString()))
            .ForMember(dest => dest.Equipment,
                opt => opt.MapFrom(src => src.Exercise.Equipment))
            .ForMember(dest => dest.VideoUrl,
                opt => opt.MapFrom(src => src.Exercise.VideoUrl));



        CreateMap<MealPlanAssignment, MealPlanDto>()
    .ForMember(dest => dest.Id,
        opt => opt.MapFrom(src => src.MealPlan.Id))
    .ForMember(dest => dest.Name,
        opt => opt.MapFrom(src => src.MealPlan.Name))
    .ForMember(dest => dest.Description,
        opt => opt.MapFrom(src => src.MealPlan.Description))
    .ForMember(dest => dest.DurationInWeeks,
        opt => opt.MapFrom(src => src.MealPlan.DurationInWeeks))
    .ForMember(dest => dest.DurationInDays,
        opt => opt.MapFrom(src => src.MealPlan.DurationInDays))
    .ForMember(dest => dest.DailyCalorieTarget,
        opt => opt.MapFrom(src => src.MealPlan.DailyCalorieTarget))
    .ForMember(dest => dest.ProteinTarget,
        opt => opt.MapFrom(src => src.MealPlan.ProteinTarget))
    .ForMember(dest => dest.CarbsTarget,
        opt => opt.MapFrom(src => src.MealPlan.CarbsTarget))
    .ForMember(dest => dest.FatTarget,
        opt => opt.MapFrom(src => src.MealPlan.FatTarget))
    .ForMember(dest => dest.Meals,
        opt => opt.MapFrom(src => src.MealPlan.Meals));

        CreateMap<Meal, MealDto>()
            .ForMember(dest => dest.MealType,
                opt => opt.MapFrom(src => src.MealType.ToString()));

        CreateMap<MealFood, MealFoodDto>();


        CreateMap<ProgressRecord, ProgressRecordDto>();


    }



}