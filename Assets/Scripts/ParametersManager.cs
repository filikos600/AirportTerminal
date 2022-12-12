using UnityEngine;

public class ParametersManager : MonoBehaviour
{
    public static ParametersManager Instance { get; private set; }

    [Header("Terminal Grid")]
    [Range(40, 100)] public int grid_width = 40;
    [Range(40, 100)] public int grid_height = 40;

    [Header("Terminal Split")]
    [Range(0, 100), Tooltip("[%] how much secuirty gates would stick to wall (-1 never; 1 always)")] public int stick_security_to_walls = 50;
    [Range(40, 70), Tooltip("[%] how much area should be for entry zone")] public int entry_split = 70;
    [Range(60, 80), Tooltip("[%] how much area should be for gates zone")] public int gates_split = 70;

    [Header("Entry Zone")]
    [Range(5, 20), Tooltip("Minimum distance between doors")] public int exit_door_step = 8;
    [Range(0, 10), Tooltip("Minimum distance from side walls to doors")] public int min_distance_door_corner = 3;
    [Range(0, 10), Tooltip("how big services can fit next to doors")] public int distance_from_entry = 6;

    [Header("Outer Gates")]
    [Range(3, 10), Tooltip("Minimum distance between gates")] public int min_gates_distance = 3;
    [Range(2, 10), Tooltip("Number of gates")] public int exit_gates= 2;
    [Range(2, 10), Tooltip("Number of gates")] public int max_service_size = 10;

    [Header("Arrivals Zone")]
    [Range(2, 10), Tooltip("Number of arrivas from Schengen")] public int arrival_gates_schen = 4;
    [Range(2, 5), Tooltip("Number of arrivas from outside Schengen")] public int arrival_gates_non_schen = 2;
    
    [Header("Services")]
    [Range(0f, 1f), Tooltip("Density of created services")] public float service_density = 0.5f;
    [Range(0f, 1f), Tooltip("Density of created benches")] public float bench_density = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
