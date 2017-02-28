using UnityEngine;
using System.Collections;

public class csMessage{

    public string Message { get; set; }
    public csMessageStatusEnum Status { get; set; }
    public csMessageTypeEnum Type { get; set; }
    public bool Enable { get; set; }
    public float Timeout { get; set; }

}
