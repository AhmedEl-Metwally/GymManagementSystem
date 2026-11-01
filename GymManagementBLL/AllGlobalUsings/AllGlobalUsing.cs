global using AutoMapper;
global using GymManagementBLL.Mapping;
global using GymManagementBLL.Plugins.Models.MemberModels;
global using GymManagementBLL.Plugins.Models.PlanModels;
global using GymManagementBLL.Plugins.Models.Plugins;
global using GymManagementBLL.Plugins.Models.SessionModels;
global using GymManagementBLL.Plugins.Models.TrainerModels;
global using GymManagementBLL.Services.AI;
global using GymManagementBLL.Services.AttachmentServices;
global using GymManagementBLL.Services.Implementation;
global using GymManagementBLL.Services.Interface;
global using GymManagementBLL.SettingsOfAttachment;
global using GymManagementBLL.ViewModels.AccountViewModels;
global using GymManagementBLL.ViewModels.AnalyticsViewModels;
global using GymManagementBLL.ViewModels.MemberPlanViewModels;
global using GymManagementBLL.ViewModels.MemberSessionViewModels;
global using GymManagementBLL.ViewModels.MemberViewModels;
global using GymManagementBLL.ViewModels.PlanViewModels;
global using GymManagementBLL.ViewModels.SessionViewModels;
global using GymManagementBLL.ViewModels.TrainerViewModels;
global using GymManagementDAL.Entities;
global using GymManagementDAL.Entities.Enums;
global using GymManagementDAL.Repositories.UnitOfWorks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.SemanticKernel;
global using Microsoft.SemanticKernel.ChatCompletion;
global using Microsoft.SemanticKernel.Connectors.OpenAI;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Globalization;
global using System.Security.Claims;
global using System.Text.Encodings.Web;
global using System.Text.Json;




