using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LoadExpressApi.Application.Common.Models;

public class Filters<T>
{
    private readonly List<GFilter<T>> _filterList;

    public Filters()
    {
        _filterList = new List<GFilter<T>>();
    }

    public void Add(bool condition, Expression<Func<T, bool>> expression)
    {
        _filterList.Add(new GFilter<T>
        {
            Condition = condition,
            Expression = expression
        });
    }

    public bool IsValid()
    {
        return _filterList.Any(f => f.Condition);
    }

    public List<GFilter<T>> Get()
    {
        return _filterList.Where(f => f.Condition).ToList();
    }
}