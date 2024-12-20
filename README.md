﻿# Final-Project-PBO-AF

## Game Bone Hunt

| Nama         | NRP        | Kelas     | 
| ---            | ---        | ----------|
| Azizah Elok Harvianti | 5025221243 | Pemrograman Berbasis Objek (B) |
| Adelia Putri Kamaski | 5025221320 | Pemrograman Berbasis Objek (B) |

## Fitur yang dikembangkan
| Fitur       | Penjelasan Fitur     
| ---         | ---        | 
| Menu              | Fitur menu akan muncul pada saat program pertama kali di-run dan terdapat tombol `Start` untuk memulai game, `Exit` untuk keluar dari game, dan  `HighScore` untuk melihat leaderboard |
| Player            | Player dalam game, yaitu anjing Shiba Inu. Dikendalikan menggunakan arrow keys atau tombol WSAD untuk bergerak. Animasi player berjalan menggunakan [sprite sheet][1] |
| Spawn Random Bone | Fitur ini akan menempatkan bone secara acak di posisi yang valid pada layar dan di-set agar bone tidak menumpuk. Apabila player mengambil `bone`, maka skor akan bertambah sebanyak 10 poin.
| Check Collision   | Mengecek tabrakan antara player dengan objek seperti `bone`, `gold`, dan `poop`. Jika tabrakan/collision terjadi, skor akan diperbarui |
| Increase Speed    | Ketika pemain mendapatkan `gold bone`, kecepatan pemain akan bertambah sementara selama 15 detik, lalu kecepatan dikembalikan ke semula​ |
| Spawn Random Gold | `Gold bone` memiliki kemungkinan muncul sebesar 10% ketika player mengambil `bone`. Mengambil `gold bone` menambah waktu permainan selama 5 detik dan meningkatkan kecepatan. |
| Spawn Random Poop | `Poop` muncul secara acak di layar. Jika player menyentuh `poop`, skor akan berkurang sebesar 5 poin, dan `poop` baru akan muncul ​|
| Score             | Skor pemain ditampilkan di layar dan diperbarui ketika player mengambil `bone` (+10), `gold` (+waktu tambahan) atau menyentuh `poop` (-5)​ |
| HighScore         | Fitur ini menyimpan dan menampilkan skor tertinggi pemain dalam file highscores.txt. Hanya 10 skor tertinggi yang disimpan dan ditampilkan​ |
| Timer             | Game memiliki batas waktu 60 detik. Timer berkurang seiring waktu dan ditampilkan di layar. Mengambil   `gold bone`   akan menambah waktu 5 detik |


## Pembagian Tugas: 
        
| Fitur       | Yang mengerjakan      
| ---         | ---        | 
| Menu              | Azizah |
| Player            | Azizah |
| Spawn Random Bone | Azizah | 
| Check Collision   | Azizah |
| Increase Speed    | Azizah |
| Spawn Random Gold | Adelia |
| Spawn Random Poop | Adelia |
| Score             | Adelia |
| HighScore         | Adelia |
| Timer             | Adelia |

Note: Semua fitur dikerjakan bersama, nama yang tertulis pada tabel merupakan nama yang mengerjakan sebagian besar dari fitur tersebut.



## Reference
[1]: https://kyafupaca.itch.io/shiba-inu "Shiba Inu Sprite Sheet"
[2]: http://example.org/ "Title"

