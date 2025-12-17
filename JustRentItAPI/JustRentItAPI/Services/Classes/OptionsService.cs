using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class OptionsService: IOptionsService
    {
        private readonly IOptionsRepository _repository;

        public OptionsService(IOptionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<List<SizeDTO>>> GetSizesAsync()
        {
            try
            {
                var sizes = await _repository.GetAllSizesAsync();
                if (sizes == null || !sizes.Any())
                {
                    return new Response<List<SizeDTO>>
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = "No sizes found."
                    };
                }

                return new Response<List<SizeDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = sizes,
                    Message = "Sizes retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<List<SizeDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving sizes: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<AgeGroupDTO>>> GetAgeGroupsAsync()
        {
            try
            {
                var ageGroups = await _repository.GetAllAgeGroupsAsync();
                if (ageGroups == null || !ageGroups.Any())
                {
                    return new Response<List<AgeGroupDTO>>
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = "No age groups found."
                    };
                }

                return new Response<List<AgeGroupDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = ageGroups,
                    Message = "Age groups retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<List<AgeGroupDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving age groups: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<CityDTO>>> GetCitiesAsync()
        {
            try
            {
                var cities = await _repository.GetAllCitiesAsync();
                if (cities == null || !cities.Any())
                {
                    return new Response<List<CityDTO>>
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = "No cities found."
                    };
                }

                return new Response<List<CityDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = cities,
                    Message = "Cities retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<List<CityDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving cities: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<EventTypeDTO>>> GetEventTypesAsync()
        {
            try
            {
                var events = await _repository.GetAllEventTypesAsync();
                if (events == null || !events.Any())
                {
                    return new Response<List<EventTypeDTO>>
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = "No event types found."
                    };
                }

                return new Response<List<EventTypeDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = events,
                    Message = "Event types retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<List<EventTypeDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving event types: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<ColorDTO>>> GetColorsAsync()
        {
            try
            {
                var colors = await _repository.GetAllColorsAsync();
                if (colors == null || !colors.Any())
                {
                    return new Response<List<ColorDTO>>
                    {
                        IsSuccess = true,
                        StatusCode = HttpStatusCode.OK,
                        Message = "No colors found."
                    };
                }

                return new Response<List<ColorDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = colors,
                    Message = "Colors retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ColorDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving colors: {ex.Message}"
                };
            }
        }
    }
}
