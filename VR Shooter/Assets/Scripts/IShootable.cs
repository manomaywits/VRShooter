using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    public int ammunition { get; set; }
    
    public void Shoot();

}
