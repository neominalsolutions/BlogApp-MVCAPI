using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
  public interface IRequestService<TRequest, TResponse> 
    where TRequest:class,new()
    where TResponse:class,new()
  {
    Task<TResponse> HandleAsync(TRequest @request);
  }
}
