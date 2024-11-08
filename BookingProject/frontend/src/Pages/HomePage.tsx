import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { ApartamentGetDto } from '../Models/Apartament';
import { getApartaments } from '../Services/ApartamentService';
import { PageResultResponse } from '../Models/Result';


const HomePage = () => {
  const [apartments, setApartments] = useState<ApartamentGetDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10); 
  const [totalPages, setTotalPages] = useState(0);

  const fetchApartments = async (page: number) => {
    setLoading(true);
    try {
      const data: PageResultResponse<ApartamentGetDto> = await getApartaments({ pageNumber: page, pageSize });
      setApartments(data.items);
      setTotalPages(data.totalPage);
    } catch (error) {
      console.error("Error fetching apartments:", error);
      toast.error("Failed to load apartments.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchApartments(pageNumber);
  }, [pageNumber]);

  const handlePreviousPage = () => {
    if (pageNumber > 1) {
      setPageNumber(pageNumber - 1);
    }
  };

  const handleNextPage = () => {
    if (pageNumber < totalPages) {
      setPageNumber(pageNumber + 1);
    }
  };

  if (loading) return <div>Loading...</div>;

  return (
    <div className="p-8">
      <h1 className="text-3xl font-bold mb-8 text-center">Apartments</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {apartments.map((apartment) => (
          <div key={apartment.apartamentId} className="bg-white rounded-lg shadow-md overflow-hidden">
            <div className="p-4">
              <div className="flex justify-between items-center mb-2">
                <h2 className="text-lg font-semibold">{apartment.address}</h2>
                <span className="text-yellow-500 font-bold">{apartment.hostName} ⭐</span>
              </div>
            </div>
          </div>
        ))}
      </div>
      
      <div className="flex justify-center mt-6">
        <button
          onClick={handlePreviousPage}
          disabled={pageNumber === 1}
          className={`px-4 py-2 mr-2 rounded ${pageNumber === 1 ? "bg-gray-300" : "bg-blue-500 text-white"}`}
        >
          Previous
        </button>
        <button
          onClick={handleNextPage}
          disabled={pageNumber === totalPages}
          className={`px-4 py-2 ml-2 rounded ${pageNumber === totalPages ? "bg-gray-300" : "bg-blue-500 text-white"}`}
        >
          Next
        </button>
      </div>
      
      <div className="text-center mt-4">
        Page {pageNumber} of {totalPages}
      </div>
    </div>
  );
};

export default HomePage;
