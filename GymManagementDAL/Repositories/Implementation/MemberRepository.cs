using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Implementation
{
    public class MemberRepository(GymDbContext _context) : IMemberRepository
    {

        public int AddMember(Member member)
        {
            _context.Members.Add(member);
            return _context.SaveChanges();
        }

        public int DeleteMember(int id)
        {
            var member =_context.Members.Find(id);
            if(member is null)
                return 0;
            _context.Members.Remove(member);
            return _context.SaveChanges();                  
        }

        public IEnumerable<Member> GetAllMembers() => _context.Members.ToList();

        public Member? GetMemberById(int id) => _context.Members.Find(id);
      
        public int UpdateMember(Member member)
        {
            _context.Members.Update(member);
            return _context.SaveChanges();  
        }
    }
}
