﻿namespace technical_tests_backend_ssr.Dtos.Common;

public class BaseResponse<T>
{
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? Code { get; set; }
    public int? TotalRows { get; set; }
    public string? Exception { get; set; }

}