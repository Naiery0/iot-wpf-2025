using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ModelViews
{
    public partial class MembersViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<Member> _members;
        public ObservableCollection<Member> Members { 
            get => _members;
            set => SetProperty(ref _members, value);
        }

        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set
            {
                SetProperty(ref _selectedMember, value);
                _isUpdate = true;
            }
        }

        private bool _isUpdate;


        public MembersViewModel(IDialogCoordinator coordinator)
        {
            this._dialogCoordinator = coordinator;
            InitVariable();
            LoadGridFromDb();
        }

        private void InitVariable()
        {
            SelectedMember = new Member
            {
                Idx = 0,
                Names = string.Empty,
                Levels = string.Empty,
                Addr = string.Empty,
                Mobile = string.Empty,
                Email = string.Empty,
            };
            _isUpdate = false;
        }
        private async void LoadGridFromDb()
        {
            try
            {
                //string connecetionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = "SELECT idx, names, levels, addr, mobile, email FROM membertbl";

                ObservableCollection<Member> member = new ObservableCollection<Member>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var idx = reader.GetInt32("idx");
                        var names = reader.GetString("names");
                        var levels = reader.GetString("levels");
                        var addr = reader.GetString("addr");
                        var mobile = reader.GetString("mobile");
                        var email = reader.GetString("email");

                        member.Add(new Member
                        {
                            Idx = idx,
                            Names = names,
                            Levels = levels,
                            Addr = addr,
                            Mobile = mobile,
                            Email = email
                        });

                    }
                }
                Members = member;
                Common.LOGGER.Info("회원 데이터 로드");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message)
                await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
                Common.LOGGER.Error(ex.Message);
            }


        }
        [RelayCommand]
        public void SetInit()
        {
            InitVariable();
        }
        [RelayCommand]
        public async void SaveData()
        {
            try
            {
                string query = string.Empty;
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();

                    if (_isUpdate)
                    {
                        query = @"UPDATE membertbl 
                                     SET names = @names, 
                                         levels = @levels, 
                                         addr = @addr, 
                                         mobile = @mobile, 
                                         email = @email 
                                   WHERE idx = @idx";
                    }
                    else
                    {
                        query = @"INSERT INTO membertbl(names, levels, addr, mobile, email) 
                                       VALUES (@names, @levels, @addr, @mobile, @email)";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@names", SelectedMember.Names);
                    cmd.Parameters.AddWithValue("@levels", SelectedMember.Levels);
                    cmd.Parameters.AddWithValue("@addr", SelectedMember.Addr);
                    cmd.Parameters.AddWithValue("@mobile", SelectedMember.Mobile);
                    cmd.Parameters.AddWithValue("@email", SelectedMember.Email);

                    if (_isUpdate) cmd.Parameters.AddWithValue("@idx", SelectedMember.Idx);

                    var resultCnt = cmd.ExecuteNonQuery();
                    if(resultCnt > 0)
                    {
                        Common.LOGGER.Info("회원 데이터 저장 완료");
                        await this._dialogCoordinator.ShowMessageAsync(this, "저장", "저장 성공");
                    }
                    else
                    {
                        Common.LOGGER.Info("회원 데이터 저장 실패");
                        await this._dialogCoordinator.ShowMessageAsync(this, "저장", "저장 실패");
                    }

                }
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            LoadGridFromDb();
        }
        [RelayCommand]
        public async void DelData()
        {
            if (!_isUpdate)
            {
                //MessageBox.Show("선택된 데이터가 아니면 삭제할 수 없습니다.");
                await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "데이터를 선택하세요");
                return;
            }
            var result = await this._dialogCoordinator.ShowMessageAsync(this, "삭제여부", "삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Negative) { return; }
            try
            {
                string query = "DELETE FROM membertbl WHERE idx = @idx";

                ObservableCollection<Member> members = new ObservableCollection<Member>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@idx", SelectedMember.Idx);

                    int resultCnt = cmd.ExecuteNonQuery(); // 삭제한 건수만큼 리턴

                    if (resultCnt > 0)
                    {
                        //MessageBox.Show("삭제성공");
                        await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제성공");
                        Common.LOGGER.Info($"멤버 {SelectedMember.Names} 삭제 성공");
                    }
                    else
                    {
                        //MessageBox.Show("삭제실패");
                        await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제실패");
                        Common.LOGGER.Warn("멤버 삭제 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            LoadGridFromDb(); // 저장하고 DB내용 다시 그리드에 그리기

        }
    }



}
