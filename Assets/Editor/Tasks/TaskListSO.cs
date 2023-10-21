using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Task List", fileName = "New Task")]
public class TaskListSO : ScriptableObject
{
    [SerializeField] List<string> tasks = new List<string>();
    public List<string> GetTasks()
    {
        return tasks;
    }

    public void AddTasks(List<string> savedTasks)
    {
        tasks.Clear();
        tasks = savedTasks;
    }

    public void AddTask(string task)
    {
        tasks.Add(task);
    }
}
