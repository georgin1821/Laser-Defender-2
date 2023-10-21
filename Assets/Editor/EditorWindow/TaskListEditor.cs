using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;

public class TaskListEditor : EditorWindow
{
    TextField taskText;
    Button addTaskButton;
    ScrollView taskListScrollView;
    ObjectField savedTasksObjectField;
    ObjectField gamePlayObject;
    Button saveProgressBtn;
    Button loadTaskButton;
    TaskListSO taskListSO;
    GameDataManager gamePlayController;
    ProgressBar taskProgressBar;

    [MenuItem("UIEditor/TaskListEditor")]
    public static void ShowExample()
    {
        TaskListEditor wnd = GetWindow<TaskListEditor>();
        wnd.titleContent = new GUIContent("TaskListEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement container = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Tasks Operations");
        container.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/EditorWindow/TaskListEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        container.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/EditorWindow/TaskListEditor.uss");
        container.styleSheets.Add(styleSheet);

        taskText = container.Q<TextField>("taskText");
        addTaskButton = container.Q<Button>("addTaskBtn");
        loadTaskButton = container.Q<Button>("loadTasksBtn");
        savedTasksObjectField = container.Q<ObjectField>("savedTasksObjectField");

        savedTasksObjectField.objectType = typeof(TaskListSO);
        gamePlayObject = container.Q<ObjectField>("testObject");
        gamePlayObject.objectType = typeof(GamePlayController);
        taskListScrollView = container.Q<ScrollView>("DisplayingTasksScroll");

        taskText.RegisterCallback<KeyDownEvent>(AddTask);
        addTaskButton.clicked += AddTask;
        loadTaskButton.clicked += LoadTasks;
        saveProgressBtn = container.Q<Button>("savedProgressBtn");
        saveProgressBtn.clicked += SaveProgress;
        taskProgressBar = container.Q<ProgressBar>("taskProgress");
    }

    private void SaveProgress()
    {
        if (taskListSO != null)
        {
            List<string> tasks = new List<string>();
            foreach (Toggle task in taskListScrollView.Children())
            {
                if (!task.value)
                {
                    tasks.Add(task.text);
                }
            }
            taskListSO.AddTasks(tasks);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadTasks();
        }
    }

    private void AddTask()
    {
        if (!string.IsNullOrEmpty(taskText.value))
        {
            taskListScrollView.Add(CreateTask(taskText.value));
            SaveTask(taskText.value);
            taskText.value = "";
            taskText.Focus();
            UpdateProgress();
        }
    }

    private void AddTask(KeyDownEvent e)
    {
        if (Event.current.Equals(Event.KeyboardEvent("Return")))
        {
            AddTask();
        }
    }
    private void LoadTasks()
    {
        taskListSO = savedTasksObjectField.value as TaskListSO;
        gamePlayController = gamePlayObject.value as GameDataManager;
        if (taskListSO != null)
        {
            taskListScrollView.Clear();
            List<string> tasks = taskListSO.GetTasks();
            foreach (var task in tasks)
            {
                taskListScrollView.Add(CreateTask(task));
            }
            UpdateProgress();
        }
    }

    private Toggle CreateTask(string task)
    {
        Toggle taskItem = new Toggle();
        taskItem.text = task;
        taskItem.RegisterValueChangedCallback(UpdateProgress);
        return taskItem;
    }
    void SaveTask(string task)
    {
        taskListSO.AddTask(task);
        EditorUtility.SetDirty(taskListSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    void UpdateProgress()
    {
        int count = 0;
        int completetd = 0;
        foreach (Toggle task in taskListScrollView.Children())
        {
            if (task.value)
            {
                completetd++;
            }
            count++;
        }

        if (count > 0)
        {
            taskProgressBar.value = completetd / (float)count;
            
        }
        else
        {
            taskProgressBar.value = 1;
        }
    }
    void UpdateProgress(ChangeEvent<bool> e)
    {
        UpdateProgress();
    }
}