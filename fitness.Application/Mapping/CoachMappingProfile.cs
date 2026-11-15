
using AutoMapper;
using fitness.Application.Features.Clients.DTOs;
using fitness.Application.Features.Coaches.Commands.CreateMeal;
using fitness.Application.Features.Coaches.Commands.CreateMealPlan;
using fitness.Application.Features.Coaches.Commands.UpdateMeal;
using fitness.Application.Features.Coaches.Commands.UpdateMealPlan;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Domain.Entities;
using ClientWorkoutDayDto = fitness.Application.Features.Clients.DTOs.WorkoutDayDto;
using ClientWorkoutExerciseDto = fitness.Application.Features.Clients.DTOs.WorkoutExerciseDto;
using ClientWorkoutPlanDto = fitness.Application.Features.Clients.DTOs.WorkoutPlanDto;

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

        // Workout Plan mappings (Client)
        CreateMap<WorkoutPlanAssignment, ClientWorkoutPlanDto>()
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

        CreateMap<WorkoutDay, ClientWorkoutDayDto>()
            .ForMember(dest => dest.Exercises,
                opt => opt.MapFrom(src => src.WorkoutExercises));

        CreateMap<WorkoutExercise, ClientWorkoutExerciseDto>()
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

        // Meal Plan mappings
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

        // Progress mappings
        CreateMap<ProgressRecord, ProgressRecordDto>();

        // Exercise mappings
        CreateMap<Exercise, ExerciseDto>();

        CreateMap<CreateMealPlanCommand, MealPlan>();

        CreateMap<MealPlan, CoachMealPlanDetailsDto>();
        CreateMap<Meal, CoachMealDto>();
        CreateMap<MealFood, CoachMealFoodDto>();


        CreateMap<UpdateMealPlanCommand, MealPlan>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CoachId, opt => opt.Ignore())
    .ForMember(dest => dest.Coach, opt => opt.Ignore())
    .ForMember(dest => dest.Meals, opt => opt.Ignore())
    .ForMember(dest => dest.Assignments, opt => opt.Ignore());


        CreateMap<CreateMealCommand, Meal>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.MealPlanId, opt => opt.Ignore())
    .ForMember(dest => dest.MealPlan, opt => opt.Ignore())
    .ForMember(dest => dest.Foods, opt => opt.Ignore());


        CreateMap<UpdateMealCommand, Meal>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.MealPlanId, opt => opt.Ignore())
    .ForMember(dest => dest.MealPlan, opt => opt.Ignore())
    .ForMember(dest => dest.Foods, opt => opt.Ignore());
    }
}
