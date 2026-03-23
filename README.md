# System Programming Tutorials

Bài tập thực hành môn Lập trình Hệ thống (System Programming).

## Yêu cầu

- .NET 8.0 SDK trở lên
- Visual Studio 2022 hoặc Visual Studio Code

## Cách mở project

### Cách 1: Mở bằng Visual Studio (Khuyến nghị)

1. Mở file `SystemProgramming.sln` bằng Visual Studio
2. Trong **Solution Explorer**, các project được sắp xếp theo từng Tutorial
3. Click chuột phải vào project cần chạy -> **Set as Startup Project**
4. Nhấn **F5** hoặc **Ctrl+F5** để chạy

### Cách 2: Chạy bằng Command Line

```bash
# Di chuyển vào thư mục project
cd Tutorial1/Question1

# Chạy project
dotnet run
```

## Cấu trúc thư mục

```
SystemProgramming.sln          # File solution chính
│
├── Tutorial1/                 # Bài 1
│   └── Question1/
│
├── Tutorial2/                 # Bài 2
│   ├── Question1/
│   ├── Question2/
│   └── Question5/
│
├── Tutorial3/                 # Bài 3 - Threading
│   ├── Question1/
│   ├── Question2/
│   ├── Question3/
│   ├── Question4/
│   └── Question5/
│
├── Tutorial4/                 # Bài 4 - Synchronization
│   ├── Question1/
│   ├── Question2/
│   ├── Question3/
│   ├── Question4/
│   └── Question5/
│
├── Tutorial5/                 # Bài 5 - IPC (Inter-Process Communication)
│   ├── Question1_ProcessA/    # Chạy ProcessA trước
│   ├── Question1_ProcessB/    # Sau đó chạy ProcessB
│   ├── Question2_Server/      # Chạy Server trước
│   ├── Question2_Client/      # Sau đó chạy Client
│   ├── Question3_Server/
│   ├── Question3_Client/
│   ├── Question4_Server/
│   ├── Question4_Client/
│   ├── Question5_Server/
│   └── Question5_Client/
│
└── Tutorial6/                 # Bài 6 - Windows Services
    ├── Question1/             # Worker Service
    ├── Question2/
    └── Question3/
```

## Hướng dẫn chạy Tutorial 5 (Client-Server)

Các bài trong Tutorial 5 yêu cầu chạy **2 ứng dụng đồng thời**:

1. **Chạy Server trước:**
   - Click chuột phải vào project Server -> Set as Startup Project
   - Nhấn F5 để chạy
   - Để cửa sổ console Server mở

2. **Chạy Client:**
   - Click chuột phải vào project Client -> Debug -> Start New Instance
   - Hoặc mở terminal mới và chạy: `dotnet run` trong thư mục Client

### Chạy nhiều project cùng lúc trong Visual Studio:

1. Click chuột phải vào **Solution** -> Properties
2. Chọn **Multiple startup projects**
3. Đặt **Action** = **Start** cho cả Server và Client
4. Server phải được đặt **trước** Client trong danh sách

## Hướng dẫn chạy Tutorial 6 (Windows Services)

Các bài trong Tutorial 6 là **Worker Services** (Background Services):

```bash
cd Tutorial6/Question1
dotnet run
```

Để cài đặt như Windows Service thật sự:
```bash
dotnet publish -c Release
sc create "MyService" binPath="đường_dẫn_đến_exe"
sc start "MyService"
```
