using System.Data;

namespace productstockingv1.ExtensionFunction;

public static class ExtenFunction
{
    public static Object StockingResponse<T>(
        string _controller = default,
        List<T>? _data = null,
        bool _count = true,
        int _statusCode = 200,
        bool _success = true,
        string _id = null
    )
    {
        var _message = $"Successfully returned with id{_id}";
        if (_success is false && _statusCode is 404)
            _message = $"{_controller} with id {_id} not found";
        return new
        {
            success = _success,
            statusCode = _statusCode,
            message = _message,
            data = _data
        };
    }

    // modified 
    public static object ResponseDefault<T>
    (
        string _controller = default,
        List<T>? _data = null,
        bool _count = true,
        int _statusCode = 200,
        bool _success = true,
        string _message = ""
    )
    {
        //modified add condition
        if (string.IsNullOrEmpty(_message))
        {
            _message = $"Successfully returned all {_controller}s";

            if (_data == null) _message = $"Ids of stockings are required ot perform querying";

            if (_count && _data.Count > 1) _message = $"Successfully returned {_data.Count} {_controller}s";

            if (_data.Count is 0 or 1) _message = $"Successfully returned {_controller}";
        }

        return new
        {
            success = _success,
            statusCode = _statusCode,
            message = _message,
            data = _data
        };
    }

    public static Object ResponseNotFound<T>
    (
        string _controller = default,
        List<T>? _data = null,
        bool _count = true,
        int _statusCode = 200,
        bool _success = true,
        string _message = ""
    )
    {
        if (string.IsNullOrEmpty(_message))
        {
            _message = $"Ware not found";

            if (_data.Count < 0) _message = $"Ids of stockings are required ot perform querying";

            if (_count && _data.Count > 1) _message = $"Successfully returned {_data.Count} {_controller}s";

            if (_data.Count is 0 or 1) _message = $"Successfully returned {_controller}";
        }


        return new
        {
            success = _success,
            statusCode = _statusCode,
            message = _message,
            data = _data
        };
    }
}