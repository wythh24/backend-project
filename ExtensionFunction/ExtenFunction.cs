using System.Data;

namespace productstockingv1.ExtensionFunction;

public static class ExtenFunction
{
    public static Object ResponseDefault<T>
    (
        string _controller = default,
        List<T> _data = null,
        bool _count = true,
        int _statusCode = 200,
        bool _success = true
    )
    {
        var _message = $"Successfully returned all {_controller}s";

        if (_count && _data.Count > 1) _message = $"Successfully returned {_data.Count} {_controller}s";

        if (_data.Count is 0 or 1)
            _message = $"Successfully returned {_controller}";

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
        List<T> _data = null,
        bool _count = true,
        int _statusCode = 404,
        bool _success = true
    )
    {
        var _message = $"Successfully returned all {_controller}s";

        if (_count)
            _message = $"Successfully returned {_data.Count} {_controller}s";

        if (_data.Count is 0 or 1)
            _message = $"Successfully returned  {_controller}";

        return new
        {
            success = _success,
            statusCode = _statusCode,
            message = _message,
            data = _data
        };
    }
}